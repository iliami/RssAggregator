using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages;

public class UpdateFeedStorage(AppDbContext dbContext) : IUpdateFeedStorage
{
    public async Task<(bool success, Guid feedId)> TryUpdateFeed(Feed feed, CancellationToken ct = default)
    {
        var feedId = feed.Id;
        var storedFeed = await dbContext.Feeds.FirstOrDefaultAsync(x => x.Id == feedId, ct);

        if (storedFeed is null)
        {
            return (false, feedId);
        }

        storedFeed.Name = feed.Name;
        storedFeed.Description = feed.Description;
        storedFeed.Url = feed.Url;
        storedFeed.LastFetchedAt = feed.LastFetchedAt;

        await dbContext.SaveChangesAsync(ct);

        return (true, feedId);
    }
}