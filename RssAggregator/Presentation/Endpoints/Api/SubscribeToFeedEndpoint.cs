using FastEndpoints;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class SubscribeToFeedEndpoint(ISubscriptionRepository SubscriptionRepository) : Endpoint<SubscribeToFeedRequest>
{
    public override void Configure()
    {
        Post("api/feeds/subscribe");
    }

    public override async Task HandleAsync(SubscribeToFeedRequest req, CancellationToken ct)
    {
        var (userId, _) = User.ToIdEmailTuple();
        await SubscriptionRepository.AttachAsync(userId, req.FeedId, ct);
    }
}