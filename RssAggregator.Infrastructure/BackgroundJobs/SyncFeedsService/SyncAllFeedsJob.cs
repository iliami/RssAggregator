using System.Xml.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        var posts = await postRepository.GetPostsUrlsAsync(stoppingToken);

        foreach (var post in posts)
        {
            var key = $"feed_{post.FeedId}";
            if (memoryCache.TryGetValue<List<string>>(key, out var urls))
                urls!.Add(post.Url);
            else
                memoryCache.Set<List<string>>(key, [post.Url]);
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
            var feedRepository = scope.ServiceProvider.GetRequiredService<IFeedRepository>();

            var feed = await feedRepository.GetByIdAsync(feedId, ct);

            var httpClient = httpClientFactory.CreateClient();
            var serializer = new XmlSerializer(typeof(RssRoot));
            var xmlString = await httpClient.GetStringAsync(feed!.Url, ct);
            using var stringReader = new StringReader(xmlString);

            var feedFromInternet = (RssRoot)serializer.Deserialize(stringReader)!;

            var feedFromInternetCategories = await GetCategoriesFromScrapedFeedAsync(feedFromInternet.Channel, feed, scope, ct);
            
            var posts = feedFromInternet.Channel.Items
                .Where(scrapedPost =>
                    (memoryCache.TryGetValue<List<string>>($"feed_{feed.Id}", out var urls) &&
                     urls!.All(url => scrapedPost.Link != url)) ||
                    urls is null)
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
                        Feed = feed
                    };
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

                await postRepository.AttachRangeAsync(posts, ct);

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

    private static async Task<Category[]> GetCategoriesFromScrapedFeedAsync(RssFeed feedFromInternet, Feed storedFeed, IServiceScope scope, CancellationToken ct = default)
    {
        var categoryRepository = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
        var allFeedCategories = await categoryRepository.GetByFeedIdWithTrackingAsync(storedFeed.Id, ct);
        
        var categories = feedFromInternet.Items
            .Select(scrapedPost => scrapedPost.Categories)
            .Aggregate(
                (acc, seq) => acc.Concat(seq).ToList())
            .Distinct()
            .Select(categoryName =>
            {
                var storedCategory = allFeedCategories.FirstOrDefault(x => x.Name == categoryName);
                if (storedCategory is not null)
                {
                    return storedCategory;
                }

                var newCategory = new Category
                {
                    Name = categoryName,
                    Feed = storedFeed
                };
                return newCategory;
            })
            .ToArray();
        
        await categoryRepository.AttachRangeAsync(categories, storedFeed.Id, ct);
        
        return categories;
    }
}