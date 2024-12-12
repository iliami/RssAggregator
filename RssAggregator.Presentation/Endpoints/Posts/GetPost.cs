using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;

namespace RssAggregator.Presentation.Endpoints.Posts;

public record GetPostResponse(
    Guid Id,
    string Title,
    string Description,
    string Category,
    DateTime PublishDate,
    string Url,
    Guid FeedId);

public class GetPost : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("posts/{id:guid}", async (
            [FromRoute]    Guid id,
            [FromServices] IPostRepository postRepository,
            [FromServices] CancellationToken ct) =>
        {
            var post = await postRepository.GetByIdAsync(id, ct);

            if (post is null) return Results.NotFound();

            var response = new GetPostResponse(
                post.Id,
                post.Title,
                post.Description,
                post.Category,
                post.PublishDate,
                post.Url,
                post.Feed.Id);
            
            return Results.Ok(response);
        }).WithTags(Tags.Posts);
    }
}