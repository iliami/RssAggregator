using System.Xml.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Domain.Entities;
using RssAggregator.Infrastructure.BackgroundJobs.SyncFeedsService.RssXmlModels;

namespace RssAggregator.Infrastructure.BackgroundJobs.SyncFeedsService;

public class SyncAllFeedsJob(
    IHttpClientFactory httpClientFactory,
    IServiceProvider serviceProvider,
    IMemoryCache memoryCache)
    : BackgroundService
{
    private const int CycleIntervalMilliseconds = 60000;
    private const string Key = "feed_{0}";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();

        var postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();
        var posts = await postRepository.GetPostsUrlsAsync(stoppingToken);
        CacheUrlsFromPosts(posts);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await FetchAllFeeds(stoppingToken);

            await Task.Delay(CycleIntervalMilliseconds, stoppingToken);
        }
    }

    private async Task FetchAllFeeds(CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        
        var feedRepository = scope.ServiceProvider.GetRequiredService<IFeedRepository>();
        var allFeeds = await feedRepository.GetFeedsIdsAsync(ct);

        await Parallel.ForEachAsync(allFeeds, ct,
            async (feed, token) => await FetchSingleFeed(feed.Id, token));
    }

    private async Task FetchSingleFeed(Guid feedId, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SyncAllFeedsJob>>();
        try
        {
            var feed = await GetStoredFeedOrThrowAsync(feedId, scope, ct);

            var feedFromInternet = await GetFeedFromInternetByUrlAsync(feed.Url, ct);

            var categoriesFromScrapedFeed = await GetAndStoreCategoriesFromScrapedFeedAsync(feedFromInternet, feed, scope, ct);

            await StorePostsFromScrapedFeedAndCacheUrlsAsync(feedFromInternet, feed, categoriesFromScrapedFeed, scope, ct);

            await UpdateStoredFeedAsync(feed, feedFromInternet, scope, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error fetching posts from the feed {feedId}.", feedId);
        }
    }

    #region Static Methods

    private static async Task UpdateStoredFeedAsync(
        Feed feed, RssFeed feedFromInternet, 
        IServiceScope scope, CancellationToken ct = default)
    {
        var feedRepository = scope.ServiceProvider.GetRequiredService<IFeedRepository>();

        feed.LastFetchedAt = DateTime.UtcNow;
        feed.Name = feedFromInternet.Title;
        feed.Description = feedFromInternet.Description;

        await feedRepository.UpdateAsync(feed.Id, feed, ct);
    }

    private static async Task<Feed> GetStoredFeedOrThrowAsync(
        Guid feedId, IServiceScope scope, 
        CancellationToken ct = default)
    {
        var feedRepository = scope.ServiceProvider.GetRequiredService<IFeedRepository>();
        return (await feedRepository.GetByIdAsync(feedId, ct)) 
               ?? throw new ArgumentNullException(nameof(feedId), $"Feed with id {feedId} not found");
    }
    
    private static async Task<Category[]> GetAndStoreCategoriesFromScrapedFeedAsync(
        RssFeed feedFromInternet, Feed storedFeed,
        IServiceScope scope, CancellationToken ct = default)
    {
        var categoryRepository = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
        var allFeedCategories = await categoryRepository.GetByFeedIdWithTrackingAsync(storedFeed.Id, ct);

        var categories = feedFromInternet.Items
            .SelectMany(x => x.Categories, 
                (_, categoryName) =>
                {
                    var storedCategory = allFeedCategories.FirstOrDefault(x => x.Name == categoryName);
                    if (storedCategory is not null)
                    {
                        storedCategory.Feed = storedFeed;
                        return storedCategory;
                    }

                    var newCategory = new Category
                    {
                        Name = categoryName,
                        NormalizedName = categoryName.ToLowerInvariant(),
                        Feed = storedFeed
                    };
                    return newCategory;
                })
            .DistinctBy(c => c.Name)
            .ToArray();

        await categoryRepository.AttachRangeAsync(categories, storedFeed.Id, ct);

        return categories;
    }

    #endregion
    
    private void CacheUrlsFromPosts(IEnumerable<object> posts)
    {
        foreach (var obj in posts)
        {
            var (key, url) = obj switch
            {
                Post post => (string.Format(Key, post.Feed.Id), post.Url),
                PostUrlDto postUrlDto => (string.Format(Key, postUrlDto.FeedId), postUrlDto.Url),
                _ => (string.Empty, string.Empty)
            };

            if (key == string.Empty)
            {
                continue;
            }

            if (memoryCache.TryGetValue<List<string>>(key, out var urls))
            {
                urls!.Add(url);
            }
            else
            {
                memoryCache.Set<List<string>>(key, [url]);
            }
        }
    }
    
    private async Task StorePostsFromScrapedFeedAndCacheUrlsAsync(
        RssFeed feedFromInternet, Feed storedFeed,
        Category[] feedFromInternetCategories, 
        IServiceScope scope, CancellationToken ct = default)
    {
        var postsToStore = feedFromInternet.Items
            .Where(scrapedPost =>
                (memoryCache.TryGetValue<List<string>>($"feed_{storedFeed.Id}", out var urls) &&
                 urls!.All(url => scrapedPost.Link != url)) || urls is null)
            .Select(scrapedPost =>
            {
                var scrapedPostCategories = feedFromInternetCategories
                    .Where(c => scrapedPost.Categories.Contains(c.Name)).ToArray();

                var scrapedPostPublishDate = DateTime.Parse(scrapedPost.PubDate).ToUniversalTime();

                return new Post
                {
                    Title = scrapedPost.Title,
                    Url = scrapedPost.Link,
                    Description = scrapedPost.Description,
                    Categories = scrapedPostCategories,
                    PublishDate = scrapedPostPublishDate,
                    Feed = storedFeed
                };
            })
            .ToArray();
        
        if (postsToStore.Length > 0)
        {
            var postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();

            await postRepository.AttachRangeAsync(postsToStore, ct);

            CacheUrlsFromPosts(postsToStore);
        }
    }

    private async Task<RssFeed> GetFeedFromInternetByUrlAsync(string feedUrl, CancellationToken ct = default)
    {
        using var httpClient = httpClientFactory.CreateClient();
        var serializer = new XmlSerializer(typeof(RssRoot));
        var xmlString = await httpClient.GetStringAsync(feedUrl, ct);
        using var stringReader = new StringReader(xmlString);

        var feedFromInternet = (RssRoot)serializer.Deserialize(stringReader)!;
        return feedFromInternet.Channel;
    }
}