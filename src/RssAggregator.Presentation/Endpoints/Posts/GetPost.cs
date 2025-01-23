using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Presentation.Endpoints.Posts;

public class GetPost : IEndpoint
{
    private class GetPostSpecification : Specification<Post>
    {
        public GetPostSpecification()
        {
            IsNoTracking = true;

            AddInclude(p => p.Feed);
            AddInclude(p => p.Categories);
        }
    }
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("posts/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IGetPostUseCase useCase,
            CancellationToken ct) =>
        {
            var specification = new GetPostSpecification();
            var request = new GetPostRequest(id, specification);
            var response = await useCase.Handle(request, ct);

            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Posts);
    }
}