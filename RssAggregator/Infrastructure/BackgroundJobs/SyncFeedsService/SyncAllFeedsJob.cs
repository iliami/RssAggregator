using System.Xml.Serialization;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Application.Abstractions.Repositories;
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SyncAllFeedsJob>>();
        var postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();
        
        var posts = await postRepository.GetPostsAsync(ct: stoppingToken);

        foreach (var post in posts)
        {
            var key = $"feed_{post.FeedId}";
            if (memoryCache.TryGetValue<List<string>>(key, out var urls))
            {
                urls!.Add(post.Url);
            }
            else
            {
                memoryCache.Set<List<string>>(key, [post.Url]);
            }
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await FetchAllFeeds(stoppingToken);
            }
            catch
            {
                logger.LogError("Error fetching all feeds.");
            }

            await Task.Delay(CycleIntervalMilliseconds, stoppingToken);
        }
    }

    private async Task FetchAllFeeds(CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();

        var feedRepository = scope.ServiceProvider.GetRequiredService<IFeedRepository>();
        
        var allFeeds = await feedRepository.GetFeedsAsync(ct: ct);

        await Parallel.ForEachAsync(allFeeds, ct,
            async (feed, token) => await FetchSingleFeed(feed.Id, token));
    }

    private async Task FetchSingleFeed(Guid feedId, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SyncAllFeedsJob>>();
        try
        {
            var feedRepository = scope.ServiceProvider.GetRequiredService<IFeedRepository>();

            var feed = await feedRepository.GetByIdAsync(feedId, ct);

            var httpClient = httpClientFactory.CreateClient();
            var serializer = new XmlSerializer(typeof(RssRoot));
            var xmlString = await httpClient.GetStringAsync(feed!.Url, ct);
            using var stringReader = new StringReader(xmlString);

            var feedFromInternet = (RssRoot)serializer.Deserialize(stringReader)!;

            var posts = feedFromInternet.Channel.Items
                .Where(scrapedPost =>
                    (memoryCache.TryGetValue<List<string>>($"feed_{feed.Id}", out var urls) &&
                     urls!.All(url => scrapedPost.Link != url)) ||
                    urls is null)
                .Select(scrapedPost => new Post
                {
                    Title = scrapedPost.Title,
                    Url = scrapedPost.Link,
                    Description = scrapedPost.Description,
                    Category = string.Join(", ", scrapedPost.Categories),
                    PublishDate = DateTime.Parse(scrapedPost.PubDate).ToUniversalTime(),
                    FeedId = feedId
                }).ToList();

            if (feed.Name != feedFromInternet.Channel.Title)
            {
                feed.Name = feedFromInternet.Channel.Title;
            }

            if (feed.Description != feedFromInternet.Channel.Description)
            {
                feed.Description = feedFromInternet.Channel.Description;
            }

            if (posts.Count != 0)
            {
                var postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();

                await postRepository.AddRangeAsync(posts, ct);

                var key = $"feed_{feed.Id}";
                foreach (var post in posts)
                {
                    if (memoryCache.TryGetValue<List<string>>(key, out var urls))
                    {
                        urls!.Add(post.Url);
                    }
                    else
                    {
                        memoryCache.Set<List<string>>(key, [post.Url]);
                    }
                }
            }

            feed.LastFetchedAt = DateTime.UtcNow;

            await feedRepository.UpdateAsync(feed.Id, feed, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error fetching posts from the feed {feedId}.", feedId);
        }
    }
}