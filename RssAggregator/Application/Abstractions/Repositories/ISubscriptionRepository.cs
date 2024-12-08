using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Abstractions.Repositories;

public interface ISubscriptionRepository
{
    Task<Subscription[]> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Subscription[]> GetByFeedIdAsync(Guid feedId, CancellationToken ct = default);
    Task AddAsync(Guid userId, Guid feedId, CancellationToken ct = default);
    Task RemoveAsync(Guid userId, Guid feedId, CancellationToken ct = default);
}