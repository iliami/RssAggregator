using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages.Feeds;

public class CreateFeedStorage(AppDbContext dbContext) : ICreateFeedStorage
{
    public async Task<Guid> CreateFeed(string name, string url, CancellationToken ct = default)
    {
        var storedFeed = await dbContext.Feeds.FirstOrDefaultAsync(f => f.Url == url, ct);
        if (storedFeed is not null)
        {
            return storedFeed.Id;
        }

        var feed = new Feed
        {
            Name = name,
            Url = url
        };
        await dbContext.Feeds.AddAsync(feed, ct);
        await dbContext.SaveChangesAsync(ct);
        return feed.Id;
    }
}