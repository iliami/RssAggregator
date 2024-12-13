using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Presentation.Endpoints.Posts;

public record GetPostsRequest(
    PaginationParams PaginationParams,
    SortingParams SortingParams);

public record GetPostsResponse(PagedResult<PostDto> Posts);

public class GetPostsFromFeed : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("feeds/{id:guid}/posts", async (
            [FromRoute]    Guid id,
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromServices] IPostRepository postRepository,
            CancellationToken ct) =>
        {
            var posts = await postRepository.GetByFeedIdAsync(
                id,
                paginationParams, 
                sortingParams, ct);
            
            var response = new GetPostsResponse(posts);
            
            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Posts);
    }
}