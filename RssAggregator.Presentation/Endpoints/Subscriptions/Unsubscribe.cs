using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Subscriptions;

public record UnsubscribeRequest(Guid FeedId);

public class Unsubscribe : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("subscribtions", async (
            [FromBody] UnsubscribeRequest request,
            [FromServices] ISubscriptionRepository subscriptionRepository,
            [FromServices] HttpContext context,
            [FromServices] CancellationToken ct) =>
        {
            var (userId, _) = context.User.ToIdEmailTuple();
            await subscriptionRepository.RemoveAsync(userId, request.FeedId, ct);
            
            context.Response.StatusCode = StatusCodes.Status204NoContent;
        }).WithTags(Tags.Subscriptions);
    }
}