using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages.Feeds;

public class UpdateFeedStorage(AppDbContext dbContext) : IUpdateFeedStorage
{
    public async Task<(bool success, Guid feedId)> TryUpdateFeed(Feed feed, CancellationToken ct = default)
    {
        var feedId = feed.Id;
        var isFeedExists = await dbContext.Feeds.AnyAsync(x => x.Id == feedId, ct);

        if (!isFeedExists)
        {
            return (false, feedId);
        }

        dbContext.Feeds.Attach(feed);
        dbContext.Feeds.Entry(feed).State = EntityState.Modified;

        await dbContext.SaveChangesAsync(ct);

        return (true, feedId);
    }
}