using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Application.UseCases.Posts.CreatePost;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Domain.Entities;
using RssAggregator.Infrastructure.BackgroundJobs.SyncAllFeedsJob.RssXmlModels;

namespace RssAggregator.Infrastructure.BackgroundJobs.SyncAllFeedsJob;

public class SyncAllFeedsJob(
    IHttpClientFactory httpClientFactory,
    IServiceProvider serviceProvider,
    IMemoryCache memoryCache)
    : BackgroundService
{
    private const int CycleIntervalMilliseconds = 60000;
    private const string UrlsPostsKey = "feed_{0}_posts_urls";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();

        await CacheAllPostsUrls(scope, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await FetchAllFeeds(stoppingToken);

            await Task.Delay(CycleIntervalMilliseconds, stoppingToken);
        }
    }

    private async Task CacheAllPostsUrls(IServiceScope scope, CancellationToken ct = default)
    {
        var getPostsRequest = new GetPostsRequest(new GetPostsSpecification());
        var getPostsUseCase = scope.ServiceProvider.GetRequiredService<IGetPostsUseCase>();
        var getPostsResponse = await getPostsUseCase.Handle(getPostsRequest, ct);

        var groupedByFeedIdPosts = getPostsResponse.Posts.GroupBy(key => key.Feed.Id);
        foreach (var groupedPosts in groupedByFeedIdPosts)
        {
            memoryCache.Set(
                string.Format(UrlsPostsKey, groupedPosts.Key),
                groupedPosts.Select(p => p.Url).ToArray(),
                DateTimeOffset.UtcNow.AddDays(1));
        }
    }

    private async Task FetchAllFeeds(CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();

        var getFeedsRequest = new GetFeedsRequest(new GetFeedsSpecification());
        var getFeedsUseCase = scope.ServiceProvider.GetRequiredService<IGetFeedsUseCase>();
        var getFeedsResponse = await getFeedsUseCase.Handle(getFeedsRequest, ct);
        
        var feeds = getFeedsResponse.Feeds;

        await Parallel.ForEachAsync(feeds, ct,
            async (feed, token) => await FetchSingleFeed(feed, token));
    }

    private async Task FetchSingleFeed(Feed feed, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SyncAllFeedsJob>>();
        try
        {
            var rssFeedFetcher = new RssFeedFetcher(httpClientFactory, feed.Url);
            var rssFeed = await rssFeedFetcher.Fetch(ct);

            var rssFeedProcessor = new RssFeedProcessor(rssFeed);
            var rssFeedProcessorResponse = rssFeedProcessor.Process();
            var allScrapedPostsInfos = rssFeedProcessorResponse.ScrapedPostsInfos;

            var scrapedPostsInfosToStore = ExcludeCachedPosts(feed.Id, allScrapedPostsInfos);

            await CreatePosts(scope, scrapedPostsInfosToStore, feed.Id, ct);

            await UpdateFeed(scope, rssFeed, feed, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error fetching posts from the feed {feedId}.", feed.Id);
        }
    }

    private ScrapedPostInfo[] ExcludeCachedPosts(Guid feedId, ScrapedPostInfo[] scrapedPostsInfos)
    {
        var urls = memoryCache.Get<string[]>(string.Format(UrlsPostsKey, feedId));

        var scrapedPostInfosToStore = scrapedPostsInfos
            .Where(postInfo => urls is null || urls.All(url => postInfo.Url != url) )
            .ToArray();

        if (scrapedPostInfosToStore.Length > 0)
        {
            var newUrls = scrapedPostInfosToStore.Select(x => x.Url);
            memoryCache.Set(
                string.Format(UrlsPostsKey, feedId), 
                urls!.Concat(newUrls).ToArray(),
                DateTimeOffset.UtcNow.AddDays(1));
        }

        return scrapedPostInfosToStore;
    }
    
    private static async Task UpdateFeed(
        IServiceScope scope, 
        RssFeed rssFeed, 
        Feed feed, 
        CancellationToken ct = default)
    {
        var updateFeedUseCase = scope.ServiceProvider.GetRequiredService<IUpdateFeedUseCase>();

        feed.LastFetchedAt = DateTimeOffset.UtcNow;
        feed.Name = rssFeed.Title;
        feed.Description = rssFeed.Description;

        var updateFeedRequest = new UpdateFeedRequest(feed);
        await updateFeedUseCase.Handle(updateFeedRequest, ct);
    }

    private static async Task CreatePosts(
        IServiceScope scope, 
        ScrapedPostInfo[] scrapedPostInfosToStore, 
        Guid feedId, 
        CancellationToken ct = default)
    {
        var createPostUseCase = scope.ServiceProvider.GetRequiredService<ICreatePostUseCase>();

        foreach (var postInfo in scrapedPostInfosToStore)
        {
            var createPostRequest = new CreatePostRequest(
                postInfo.Title,
                postInfo.Description,
                postInfo.Categories,
                postInfo.PublishDate,
                postInfo.Url,
                feedId);

            await createPostUseCase.Handle(createPostRequest, ct);
        }
    }

    private class GetFeedsSpecification : Specification<Feed>
    {
        public GetFeedsSpecification()
        {
            IsNoTracking = true;
        }
    }

    private class GetPostsSpecification : Specification<Post>
    {
        public GetPostsSpecification()
        {
            IsNoTracking = true;

            AddInclude(post => post.Feed);
        }
    }
}