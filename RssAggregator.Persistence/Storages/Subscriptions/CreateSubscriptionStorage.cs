using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;

namespace RssAggregator.Persistence.Storages.Subscriptions;

public class CreateSubscriptionStorage(AppDbContext dbContext) : ICreateSubscriptionStorage
{
    public Task<bool> IsFeedExist(Guid feedId, CancellationToken ct = default)
        => dbContext.Feeds.AnyAsync(f => f.Id == feedId, ct);

    public async Task<bool> CreateSubscription(Guid userId, Guid feedId, CancellationToken ct = default)
    {
        var appUser = await dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (appUser == null)
        {
            return false;
        }

        var feed = await dbContext.Feeds.FirstOrDefaultAsync(f => f.Id == feedId, ct);
        if (feed == null)
        {
            return false;
        }

        await dbContext.Entry(feed)
            .Collection(f => f.Subscribers)
            .LoadAsync(ct);

        var isSubscribedToFeed = feed.Subscribers.Any(s => s.Id == userId);

        if (isSubscribedToFeed)
        {
            return true;
        }

        feed.Subscribers.Add(appUser);

        await dbContext.SaveChangesAsync(ct);

        return true;
    }
}