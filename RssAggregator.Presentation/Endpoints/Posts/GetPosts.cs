using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Posts.GetPosts;

namespace RssAggregator.Presentation.Endpoints.Posts;

public class GetPosts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("posts", async (
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromQuery] string[]? categories,
            [FromServices] IGetPostsUseCase useCase,
            CancellationToken ct) =>
        {
            var filterParams = new PostFilterParams(categories ?? []);
            var request = new GetPostsRequest(
                paginationParams,
                sortingParams,
                filterParams);
            
            var posts = await useCase.Handle(request, ct);
            
            return Results.Ok(posts);
        }).AllowAnonymous().WithTags(EndpointsTags.Posts);
    }
}