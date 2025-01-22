using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public interface IGetFeedStorage
{
    Task<(bool success, Feed feed)> TryGetFeed(
        Specification<Feed> specification,
        CancellationToken ct = default);
}