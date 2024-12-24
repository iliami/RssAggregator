using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages;

public class GetFeedStorage(AppDbContext dbContext) : IGetFeedStorage
{
    public async Task<(bool success, Feed feed)> TryGetFeed(Guid feedId, CancellationToken ct = default)
    {
        var success = await dbContext.Feeds.AnyAsync(f => f.Id == feedId, ct);
        
        if (!success)
        {
            return (false, null!);
        }
        
        var feed = await dbContext.Feeds
            .Where(f => f.Id == feedId)
            .FirstAsync(ct);
        
        return (true, feed);
    }
}