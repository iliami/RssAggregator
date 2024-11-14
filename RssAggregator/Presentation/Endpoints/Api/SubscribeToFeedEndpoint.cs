using FastEndpoints;
using RssAggregator.Application.Abstractions;
using RssAggregator.Domain.Entities;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class SubscribeToFeedEndpoint(IAppDbContext DbContext) : Endpoint<SubscribeToFeedRequest>
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