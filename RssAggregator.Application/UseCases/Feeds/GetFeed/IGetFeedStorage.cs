using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public interface IGetFeedStorage
{
    Task<(bool success, TProjection feed)> TryGetFeed<TProjection>(
        Specification<Feed, TProjection> specification, 
        CancellationToken ct = default)
        where TProjection : class;
}