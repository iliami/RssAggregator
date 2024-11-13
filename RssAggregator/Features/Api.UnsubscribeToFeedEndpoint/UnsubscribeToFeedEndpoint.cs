using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Extensions;
using RssAggregator.Persistence;

namespace RssAggregator.Features.Api.UnsubscribeToFeedEndpoint;

public class UnsubscribeToFeedEndpoint(AppDbContext DbContext) : Endpoint<UnsubscribeToFeedRequest>
{
    public override void Configure()
    {
        Post("api/feeds/unsubscribe");
    }

    public override async Task HandleAsync(UnsubscribeToFeedRequest req, CancellationToken ct)
    {
        var (userId, _) = User.ToIdNameTuple();
        
        var subscription = await DbContext.Subscriptions.FirstOrDefaultAsync(s => s.AppUserId == userId && s.FeedId == req.FeedId, ct);
        if (subscription is null)
        {
            ThrowError("Subscription not found", StatusCodes.Status404NotFound);
        }
        
        DbContext.Entry(subscription).State = EntityState.Deleted;
        await DbContext.SaveChangesAsync(ct);
    }
}