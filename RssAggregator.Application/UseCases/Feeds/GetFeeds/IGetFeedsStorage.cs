using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public interface IGetFeedsStorage
{
    Task<TProjection[]> GetFeeds<TProjection>(
        Specification<Feed, TProjection> specification, 
        CancellationToken ct = default) 
        where TProjection : class;
}