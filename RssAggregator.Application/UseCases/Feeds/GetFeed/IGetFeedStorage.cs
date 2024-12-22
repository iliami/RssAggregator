using RssAggregator.Application.Models.DTO;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public interface IGetFeedStorage
{
    Task<(bool success, FeedDto feed)> TryGetFeed(Guid feedId, CancellationToken ct = default);
}