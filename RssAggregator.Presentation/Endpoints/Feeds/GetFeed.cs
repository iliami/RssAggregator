using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.UseCases.Feeds.GetFeed;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public class GetFeed : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("feeds/{id:guid}", async (
            [FromRoute]    Guid id,
            [FromServices] IGetFeedUseCase useCase,
            CancellationToken ct) =>
        {
            var request = new GetFeedRequest(id);
            var response = useCase.Handle(request, ct);
            return Results.Ok(response);
        }).RequireAuthorization().WithTags(EndpointsTags.Feeds);
    }
}