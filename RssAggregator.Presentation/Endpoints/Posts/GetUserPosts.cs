using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Posts;

public record GetUserPostsRequest(
    PaginationParams PaginationParams,
    SortingParams SortingParams);

public record GetUserPostsResponse(PagedResult<PostDto> Posts);

public class GetUserPosts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("posts/me", async (
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromServices] IPostRepository postRepository,
            [FromServices] HttpContext context,
            CancellationToken ct) =>
        {
            var (userId, _) = context.User.ToIdEmailTuple();
            
            var posts = await postRepository.GetByUserIdAsync(
                userId,
                paginationParams, 
                sortingParams, ct);
            
            var response = new GetUserPostsResponse(posts);
            
            return Results.Ok(response);
        }).RequireAuthorization().WithTags(EndpointsTags.Posts);
    }
}