using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetUserFeeds;

public interface IGetUserFeedsStorage
{
    Task<Feed[]> GetUserFeeds(Guid userId, Specification<Feed> specification, CancellationToken ct = default);
}