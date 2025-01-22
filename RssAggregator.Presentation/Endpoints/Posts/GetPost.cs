using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.UseCases.Posts.GetPost;

namespace RssAggregator.Presentation.Endpoints.Posts;

public class GetPost : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("posts/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IGetPostUseCase useCase,
            CancellationToken ct) =>
        {
            var request = new GetPostRequest(id);
            var response = await useCase.Handle(request, ct);

            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Posts);
    }
}