using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Specifications;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages.Feeds;

public class GetFeedStorage(AppDbContext dbContext) : IGetFeedStorage
{
    public async Task<(bool success, Feed feed)> TryGetFeed(
        Specification<Feed> specification,
        CancellationToken ct = default)
    {
        var success = await dbContext.Feeds.AnyAsync(specification.Criteria!, ct);

        if (!success)
        {
            return (false, null!);
        }

        var feed = await dbContext.Feeds
            .EvaluateSpecification(specification)
            .FirstAsync(ct);

        return (true, feed);
    }
}