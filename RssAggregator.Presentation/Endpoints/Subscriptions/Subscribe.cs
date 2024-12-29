using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;

namespace RssAggregator.Presentation.Endpoints.Subscriptions;

public class Subscribe : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("subscriptions", async (
            [FromBody]     CreateSubscriptionRequest request,
            [FromServices] ICreateSubscriptionUseCase useCase,
            HttpContext context) =>
        {
            await useCase.Handle(request, context.RequestAborted);
            context.Response.StatusCode = StatusCodes.Status204NoContent;
        }).RequireAuthorization().WithTags(EndpointsTags.Subscriptions);
    }
}