using System.Xml.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RssAggregator.Application.Specifications;
using RssAggregator.Application.UseCases.Categories.CreateCategory;
using RssAggregator.Application.UseCases.Categories.GetCategories;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Domain.Entities;
using RssAggregator.Infrastructure.SyncAllFeedsJob.RssXmlModels;

namespace RssAggregator.Infrastructure.SyncAllFeedsJob;

public class SyncAllFeedsJob(
    IHttpClientFactory httpClientFactory,
    IServiceProvider serviceProvider,
    IMemoryCache memoryCache)
    : BackgroundService
{
    private const int CycleIntervalMilliseconds = 60000;
    private const string CacheUrlsPostsKey = "feed_{0}_posts_urls";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();

        var getFeedsUseCase = scope.ServiceProvider.GetRequiredService<IGetFeedsUseCase>();

        var getFeedsForCachePostsUrlsRequest = new GetFeedsRequest(new GetFeedsForCachePostsUrlsSpecification());
        var getFeedsForCachePostsUrlsResponse =
            await getFeedsUseCase.Handle(getFeedsForCachePostsUrlsRequest, stoppingToken);

        foreach (var feed in getFeedsForCachePostsUrlsResponse.Feeds)
        {
            memoryCache.Set(
                string.Format(CacheUrlsPostsKey, feed.Id),
                feed.Posts.Select(p => p.Url).ToArray());
        }

        var getFeedsRequest = new GetFeedsRequest(new GetFeedsSpecification());
        while (!stoppingToken.IsCancellationRequested)
        {
            var getFeedsResponse = await getFeedsUseCase.Handle(getFeedsRequest, stoppingToken);

            await Parallel.ForEachAsync(getFeedsResponse.Feeds, stoppingToken,
                async (feed, token) => await FetchSingleFeed(feed, token));

            await Task.Delay(CycleIntervalMilliseconds, stoppingToken);
        }
    }

    private async Task FetchSingleFeed(Feed feed, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SyncAllFeedsJob>>();
        try
        {
            var rssFeed = await FetchFeed(feed.Url, ct);

            var scrapedPostsInfosToStore = GetNewlyPostsFromRssFeed(feed.Id, rssFeed);

            var categories = await ProcessCategories(scope, feed, scrapedPostsInfosToStore, ct);

            var posts = scrapedPostsInfosToStore.Select(postInfo => new Post
            {
                Title = postInfo.Title,
                Description = postInfo.Description,
                PublishDate = postInfo.PublishDate,
                Url = postInfo.Url,
                Feed = feed,
                Categories = categories.Where(c =>
                        postInfo.Categories.Contains(c.NormalizedName, StringComparer.InvariantCultureIgnoreCase))
                    .ToArray()
            });

            await UpdateFeed(scope, rssFeed, posts, feed, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error fetching posts from the feed {feedId}.", feed.Id);
        }
    }

    private static async Task<Category[]> ProcessCategories(
        IServiceScope scope,
        Feed feed,
        ScrapedPostInfo[] scrapedPostsInfosToStore,
        CancellationToken ct = default)
    {
        var categoriesNames = scrapedPostsInfosToStore
            .SelectMany(p => p.Categories)
            .Distinct()
            .ToArray();

        var getCategoriesRequest = new GetCategoriesRequest(new GetCategoriesSpecification(feed.Id));
        var getCategoriesUseCase = scope.ServiceProvider.GetRequiredService<IGetCategoriesUseCase>();
        var getCategoriesResponse = await getCategoriesUseCase.Handle(getCategoriesRequest, ct);

        var isAnyNewCategory = false;
        var categories = categoriesNames.Select(name =>
        {
            var storedCategory = getCategoriesResponse.Categories.FirstOrDefault(x =>
                x.NormalizedName.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (storedCategory is not null)
            {
                return storedCategory;
            }

            isAnyNewCategory = true;
            return new Category
            {
                Name = name,
                NormalizedName = name.ToLowerInvariant(),
                Feed = feed
            };
        }).ToArray();

        if (!isAnyNewCategory)
        {
            return categories;
        }

        var createCategoryUseCase = scope.ServiceProvider.GetRequiredService<ICreateCategoryUseCase>();

        foreach (var category in categories)
        {
            if (category.Id != Guid.Empty)
            {
                continue;
            }

            var createCategoryRequest = new CreateCategoryRequest(category.Name, category.Feed.Id);
            var createCategoryResponse = await createCategoryUseCase.Handle(createCategoryRequest, ct);

            category.Id = createCategoryResponse.Id;
        }

        return categories;
    }

    private ScrapedPostInfo[] GetNewlyPostsFromRssFeed(Guid feedId, RssFeed rssFeed)
    {
        var urls = memoryCache.Get<string[]>(string.Format(CacheUrlsPostsKey, feedId));

        var scrapedPostInfosToStore = rssFeed.Items
            .Select(scrapedPost =>
            {
                var scrapedPostPublishDate = DateTime
                    .Parse(scrapedPost.PublishDate)
                    .ToUniversalTime();

                return new ScrapedPostInfo(
                    scrapedPost.Title,
                    scrapedPost.Description,
                    scrapedPost.Categories.ToArray(),
                    scrapedPostPublishDate,
                    scrapedPost.Url);
            })
            .Where(postInfo => urls is null || urls.All(url => postInfo.Url != url))
            .ToArray();

        if (scrapedPostInfosToStore.Length > 0)
        {
            var newUrls = scrapedPostInfosToStore.Select(x => x.Url);
            memoryCache.Set(
                string.Format(CacheUrlsPostsKey, feedId),
                urls!.Concat(newUrls).ToArray());
        }

        return scrapedPostInfosToStore;
    }

    private async Task<RssFeed> FetchFeed(string feedUrl, CancellationToken ct = default)
    {
        using var httpClient = httpClientFactory.CreateClient();
        var serializer = new XmlSerializer(typeof(RssRoot));
        var xmlString = await httpClient.GetStringAsync(feedUrl, ct);
        using var stringReader = new StringReader(xmlString);

        var feedFromInternet = (RssRoot)serializer.Deserialize(stringReader)!;
        return feedFromInternet.Channel;
    }

    private static async Task UpdateFeed(
        IServiceScope scope,
        RssFeed rssFeed,
        IEnumerable<Post> newPosts,
        Feed feed,
        CancellationToken ct = default)
    {
        var updateFeedUseCase = scope.ServiceProvider.GetRequiredService<IUpdateFeedUseCase>();

        feed.LastFetchedAt = DateTimeOffset.UtcNow;
        if (feed.Name != rssFeed.Name)
        {
            feed.Name = rssFeed.Name;
        }

        if (feed.Description != rssFeed.Description)
        {
            feed.Description = rssFeed.Description;
        }

        foreach (var newPost in newPosts)
        {
            feed.Posts.Add(newPost);
        }

        var updateFeedRequest = new UpdateFeedRequest(feed);
        await updateFeedUseCase.Handle(updateFeedRequest, ct);
    }

    private record ScrapedPostInfo(
        string Title,
        string Description,
        string[] Categories,
        DateTime PublishDate,
        string Url);

    private class GetFeedsSpecification : Specification<Feed>
    {
        public GetFeedsSpecification()
        {
            IsNoTracking = true;
        }
    }

    private class GetFeedsForCachePostsUrlsSpecification : Specification<Feed>
    {
        public GetFeedsForCachePostsUrlsSpecification()
        {
            IsNoTracking = true;

            AddInclude(feed => feed.Posts);
        }
    }

    private class GetCategoriesSpecification : Specification<Category>
    {
        public GetCategoriesSpecification(Guid feedId)
        {
            IsNoTracking = true;

            Criteria = category => category.Feed.Id == feedId;
        }
    }
}