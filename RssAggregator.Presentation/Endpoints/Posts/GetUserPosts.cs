using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Posts;

public record GetUserPostsResponse(PagedResult<PostDto> Posts);

public class GetUserPosts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("posts/me", async (
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromQuery] string[]? categories,
            [FromServices] IPostRepository postRepository,
            ClaimsPrincipal user,
            CancellationToken ct) =>
        {
            var (userId, _) = user.ToIdEmailTuple();
            
            var postFilterParams = new PostFilterParams
            {
                Categories = categories ?? [],
            };
            
            var posts = await postRepository.GetByUserIdAsync(
                userId,
                paginationParams, 
                sortingParams, 
                postFilterParams, ct);
            
            var response = new GetUserPostsResponse(posts);
            
            return Results.Ok(response);
        }).RequireAuthorization().WithTags(EndpointsTags.Posts);
    }
}