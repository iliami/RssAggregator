using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages;

public class GetFeedStorage(AppDbContext dbContext) : IGetFeedStorage
{
    public async Task<(bool success, TProjection feed)> TryGetFeed<TProjection>(
        Specification<Feed, TProjection> specification, 
        CancellationToken ct = default)
    where TProjection : class
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