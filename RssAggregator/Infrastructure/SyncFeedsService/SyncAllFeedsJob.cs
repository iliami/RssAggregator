using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Domain.Entities;
using RssAggregator.Infrastructure.SyncFeedsService.RssXmlModels;

namespace RssAggregator.Infrastructure.SyncFeedsService;

public class SyncAllFeedsJob(
    IHttpClientFactory httpClientFactory,
    IServiceProvider serviceProvider)
    : BackgroundService
{
    private const int CycleIntervalMilliseconds = 60000;
    private Dictionary<Guid, HashSet<string>> _feedIdPostIds = null!;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        _feedIdPostIds = await dbContext.Feeds
            .AsNoTracking()
            .Include(f => f.Posts)
            .ToDictionaryAsync(
                k => k.Id,
                v => v.Posts.Select(p => p.Url).ToHashSet(), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await FetchAllFeeds(stoppingToken);

            await Task.Delay(CycleIntervalMilliseconds, stoppingToken);
        }
    }

    private async Task FetchAllFeeds(CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        var allFeeds = await dbContext.Feeds.ToListAsync(ct);

        await Parallel.ForEachAsync(allFeeds, ct,
            async (feed, token) => await FetchSingleFeed(feed.Id, token));
    }

    private async Task FetchSingleFeed(Guid feedId, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        
        var feed = await dbContext.Feeds.FirstAsync(f => f.Id == feedId, ct);
        
        var httpClient = httpClientFactory.CreateClient();
        var serializer = new XmlSerializer(typeof(RssRoot));
        var xmlString = await httpClient.GetStringAsync(feed.Url, ct);
        using var stringReader = new StringReader(xmlString);
        
        var feedFromInternet = (RssRoot)serializer.Deserialize(stringReader)!;
        
        var posts = feedFromInternet.Channel.Items
            .Where(scrapedPost => 
                (_feedIdPostIds.TryGetValue(feed.Id, out var urls) && 
                 urls.All(url => scrapedPost.Link != url)) ||
                urls is null)
            .Select(scrapedPost => new Post
            {
                Title = scrapedPost.Title,
                Url = scrapedPost.Link,
                Description = scrapedPost.Description,
                PublishDate = DateTime.Parse(scrapedPost.PubDate).ToUniversalTime(),
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
            feed.Posts.AddRange(posts);

            foreach (var post in posts)
            {
                if (_feedIdPostIds.TryGetValue(feed.Id, out var urls))
                {
                    urls.Add(post.Url);
                }
                else
                {
                    _feedIdPostIds[feed.Id] = [post.Url];
                }
            }
        }
        
        feed.LastFetchedAt = DateTime.UtcNow;
        
        await dbContext.SaveChangesAsync(ct);
    }
}