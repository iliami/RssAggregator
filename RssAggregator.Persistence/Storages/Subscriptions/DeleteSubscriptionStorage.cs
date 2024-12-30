using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;

namespace RssAggregator.Persistence.Storages.Subscriptions;

public class DeleteSubscriptionStorage(AppDbContext dbContext) : IDeleteSubscriptionStorage
{
    public Task<bool> IsFeedExist(Guid feedId, CancellationToken ct = default)
        => dbContext.Feeds.AnyAsync(f => f.Id == feedId, ct);

    public async Task<bool> DeleteSubscription(Guid userId, Guid feedId, CancellationToken ct = default)
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

        var isNotSubscribedToFeed = feed.Subscribers.All(s => s.Id != userId);

        if (isNotSubscribedToFeed)
        {
            return true;
        }

        feed.Subscribers.Remove(appUser);

        await dbContext.SaveChangesAsync(ct);

        return true;
    }
}