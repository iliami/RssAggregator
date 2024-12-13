using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Subscriptions;

public record SubscribeRequest(Guid FeedId);

public class Subscribe : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("subscriptions", async (
            [FromBody]     SubscribeRequest request,
            [FromServices] ISubscriptionRepository subscriptionRepository,
            HttpContext context,
            CancellationToken ct) =>
        {
            var (userId, _) = context.User.ToIdEmailTuple();
            await subscriptionRepository.AttachAsync(userId, request.FeedId, ct);
            
            context.Response.StatusCode = StatusCodes.Status204NoContent;
        }).RequireAuthorization().WithTags(EndpointsTags.Subscriptions);
    }
}