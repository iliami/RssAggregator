// using FastEndpoints;
// using RssAggregator.Application.Abstractions.Repositories;
// using RssAggregator.Presentation.Extensions;
//
// namespace RssAggregator.Presentation.Endpoints.Api;
//
// public record UnsubscribeToFeedRequest(Guid FeedId);
//
// public class UnsubscribeToFeedEndpoint(ISubscriptionRepository SubscriptionRepository)
//     : Endpoint<UnsubscribeToFeedRequest>
// {
//     public override void Configure()
//     {
//         Post("api/feeds/unsubscribe");
//     }
//
//     public override async Task HandleAsync(UnsubscribeToFeedRequest req, CancellationToken ct)
//     {
//         var (userId, _) = User.ToIdEmailTuple();
//         await SubscriptionRepository.RemoveAsync(userId, req.FeedId, ct);
//     }
// }