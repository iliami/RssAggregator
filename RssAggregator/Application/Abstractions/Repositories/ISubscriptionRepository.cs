using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Abstractions.Repositories;

public interface ISubscriptionRepository
{
    Task<IEnumerable<Subscription>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Guid userId, Guid feedId, CancellationToken ct = default);
    Task RemoveAsync(Guid userId, Guid feedId, CancellationToken ct = default);
}