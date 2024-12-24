using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public interface IGetFeedStorage
{
    Task<(bool success, Feed feed)> TryGetFeed(Guid feedId, CancellationToken ct = default);
}