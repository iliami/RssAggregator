using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Repositories;

public class SubscriptionRepository(IAppDbContext DbContext) : ISubscriptionRepository
{
    public Task<Subscription[]> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => DbContext.Subscriptions.AsNoTracking()
            .Where(s => s.AppUserId == userId)
            .ToArrayAsync(ct);

    public Task<Subscription[]> GetByFeedIdAsync(Guid feedId, CancellationToken ct = default)
        => DbContext.Subscriptions.AsNoTracking()
            .Where(s => s.FeedId == feedId)
            .ToArrayAsync(ct);

    public async Task AddAsync(Guid userId, Guid feedId, CancellationToken ct = default)
    {
        var subscription = new Subscription
        {
            AppUserId = userId,
            FeedId = feedId
        };

        await DbContext.Subscriptions.AddAsync(subscription, ct);
        await DbContext.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Guid userId, Guid feedId, CancellationToken ct = default)
    {
        var subscription = await DbContext.Subscriptions
            .FirstOrDefaultAsync(s => s.AppUserId == userId && s.FeedId == feedId, ct);

        if (subscription is not null)
        {
            DbContext.Subscriptions.Entry(subscription).State = EntityState.Deleted;
            await DbContext.SaveChangesAsync(ct);
        }
    }
}