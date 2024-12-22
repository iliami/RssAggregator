using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.UseCases.Feeds.GetFeed;

namespace RssAggregator.Persistence.Storages;

public class GetFeedStorage(AppDbContext dbContext) : IGetFeedStorage
{
    public async Task<(bool success, FeedDto feed)> TryGetFeed(Guid feedId, CancellationToken ct = default)
    {
        var success = await dbContext.Feeds.AnyAsync(f => f.Id == feedId, ct);
        
        if (!success)
        {
            return (false, null!);
        }
        
        var storedFeed = await dbContext.Feeds
            .Where(f => f.Id == feedId)
            .Include(feed => feed.Posts)
            .Include(feed => feed.Subscribers)
            .FirstAsync(ct);

        var feed = new FeedDto(
            storedFeed.Id,
            storedFeed.Name,
            storedFeed.Description,
            storedFeed.Url,
            storedFeed.Subscribers.Count,
            storedFeed.Posts.Count
        );
        
        return (true, feed);
    }
}