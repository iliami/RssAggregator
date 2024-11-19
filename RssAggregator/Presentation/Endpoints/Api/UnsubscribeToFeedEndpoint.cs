using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class UnsubscribeToFeedEndpoint(IAppDbContext DbContext) : Endpoint<UnsubscribeToFeedRequest>
{
    public override void Configure()
    {
        Post("api/feeds/unsubscribe");
    }

    public override async Task HandleAsync(UnsubscribeToFeedRequest req, CancellationToken ct)
    {
        var (userId, _) = User.ToIdEmailTuple();
        
        var subscription = await DbContext.Subscriptions.FirstOrDefaultAsync(s => s.AppUserId == userId && s.FeedId == req.FeedId, ct);
        if (subscription is null)
        {
            ThrowError("Subscription not found", StatusCodes.Status404NotFound);
        }
        
        DbContext.Subscriptions.Remove(subscription);
        await DbContext.SaveChangesAsync(ct);
    }
}