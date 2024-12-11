using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;

namespace RssAggregator.Persistence.Repositories;

public class SubscriptionRepository(AppDbContext DbContext) : ISubscriptionRepository
{
    public Task<SubscriptionDto[]> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return DbContext.Feeds.AsNoTracking()
            .Where(f => f.Subscribers.Any(u => u.Id == userId))
            .SelectMany(
                f => f.Subscribers,
                (feed, user) => new SubscriptionDto(user, feed))
            .ToArrayAsync(ct);
    }

    public Task<SubscriptionDto[]> GetByFeedIdAsync(Guid feedId, CancellationToken ct = default)
    {
        return DbContext.Feeds.AsNoTracking()
            .Where(f => f.Id == feedId)
            .SelectMany(
                f => f.Subscribers,
                (feed, user) => new SubscriptionDto(user, feed))
            .ToArrayAsync(ct);
    }

    public async Task AttachAsync(Guid userId, Guid feedId, CancellationToken ct = default)
    {
        var appUser = await DbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == userId, ct);
        var feed = await DbContext.Feeds.FirstOrDefaultAsync(f => f.Id == feedId, ct);

        feed?.Subscribers.Add(appUser);

        await DbContext.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Guid userId, Guid feedId, CancellationToken ct = default)
    {
        var appUser = await DbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == userId, ct);
        var feed = await DbContext.Feeds.FirstOrDefaultAsync(f => f.Id == feedId, ct);

        feed?.Subscribers.Remove(appUser);

        await DbContext.SaveChangesAsync(ct);
    }
}