using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Subscriptions;

public class Unsubscribe : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("subscriptions", async (
            [FromBody]     DeleteSubscriptionRequest request,
            [FromServices] IDeleteSubscriptionUseCase useCase,
            HttpContext context) =>
        {
            var response = await useCase.Handle(request, context.RequestAborted);
            
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return response;
        }).RequireAuthorization().WithTags(EndpointsTags.Subscriptions);
    }
}