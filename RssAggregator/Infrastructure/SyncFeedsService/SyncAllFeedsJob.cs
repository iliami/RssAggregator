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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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

        var allFeeds = dbContext.Feeds
            .AsNoTracking()
            .ToList();

        await Parallel.ForEachAsync(allFeeds, ct,
            async (feed, token) => await FetchSingleFeed(feed.Id, token));
    }

    private async Task FetchSingleFeed(Guid feedId, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        var dbFeed = await dbContext.Feeds
            .Include(x => x.Posts)
            .FirstAsync(x => x.Id == feedId, ct);

        var httpClient = httpClientFactory.CreateClient();
        var serializer = new XmlSerializer(typeof(RssRoot));

        var xmlString = await httpClient.GetStringAsync(dbFeed.Url, ct);

        using var reader = new StringReader(xmlString);

        var feedFromInternet = (RssRoot)serializer.Deserialize(reader)!;

        var posts = feedFromInternet.Channel.Items
            .Where(scrapedPost =>
                dbFeed.Posts.All(dbPost => scrapedPost.Link != dbPost.Url))
            .Select(scrapedPost => new Post
            {
                Title = scrapedPost.Title,
                Url = scrapedPost.Link,
                Description = scrapedPost.Description,
                PublishDate = DateTime.Parse(scrapedPost.PubDate).ToUniversalTime(),
            }).ToList();

        dbFeed.LastFetchedAt = DateTime.UtcNow;
        dbFeed.Posts.AddRange(posts);

        await dbContext.SaveChangesAsync(ct);
    }
}