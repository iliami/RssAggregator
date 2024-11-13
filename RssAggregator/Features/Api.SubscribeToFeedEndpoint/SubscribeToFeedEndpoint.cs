using FastEndpoints;
using RssAggregator.Extensions;
using RssAggregator.Persistence;
using RssAggregator.Persistence.Entities;

namespace RssAggregator.Features.Api.SubscribeToFeedEndpoint;

public class SubscribeToFeedEndpoint(AppDbContext DbContext) : Endpoint<SubscribeToFeedRequest>
{
    public override void Configure()
    {
        Post("api/feeds/subscribe");
    }

    public override async Task HandleAsync(SubscribeToFeedRequest req, CancellationToken ct)
    {
        var (userId, _) = User.ToIdNameTuple();
        var subscription = new Subscription
        {
            AppUserId = userId,
            FeedId = req.FeedId
        };
        
        await DbContext.Subscriptions.AddAsync(subscription, ct);
        await DbContext.SaveChangesAsync(ct);
    }
}