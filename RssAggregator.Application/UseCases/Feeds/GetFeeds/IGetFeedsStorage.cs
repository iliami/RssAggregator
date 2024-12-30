using RssAggregator.Application.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public interface IGetFeedsStorage
{
    Task<Feed[]> GetFeeds(
        Specification<Feed> specification,
        CancellationToken ct = default);
}