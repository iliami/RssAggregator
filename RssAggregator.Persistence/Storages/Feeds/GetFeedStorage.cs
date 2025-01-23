using Microsoft.EntityFrameworkCore;
using RssAggregator.Application;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages.Feeds;

public class GetFeedStorage(AppDbContext dbContext) : IGetFeedStorage
{
    public async Task<(bool success, Feed feed)> TryGetFeed(
        Guid feedId,
        Specification<Feed> specification,
        CancellationToken ct = default)
    {
        var success = await dbContext.Feeds.AnyAsync(f => f.Id == feedId, ct);

        if (!success)
        {
            return (false, null!);
        }

        var feed = await dbContext.Feeds
            .Where(f => f.Id == feedId)
            .EvaluateSpecification(specification)
            .FirstAsync(ct);

        return (true, feed);
    }
}