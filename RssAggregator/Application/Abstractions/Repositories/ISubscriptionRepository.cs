using RssAggregator.Application.Models.DTO;

namespace RssAggregator.Application.Abstractions.Repositories;

public interface ISubscriptionRepository
{
    Task<SubscriptionDto[]> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<SubscriptionDto[]> GetByFeedIdAsync(Guid feedId, CancellationToken ct = default);
    Task AttachAsync(Guid userId, Guid feedId, CancellationToken ct = default);
    Task RemoveAsync(Guid userId, Guid feedId, CancellationToken ct = default);
}