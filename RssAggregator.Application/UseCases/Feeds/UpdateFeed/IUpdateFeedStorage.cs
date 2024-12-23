using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.UpdateFeed;

public interface IUpdateFeedStorage
{
    Task<(bool success, Guid feedId)> TryUpdateFeed(Feed feed, CancellationToken ct = default);
}