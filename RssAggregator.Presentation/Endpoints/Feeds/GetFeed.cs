using Microsoft.AspNetCore.Mvc;
using Namotion.Reflection;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public record GetFeedResponse(FeedDto Feed);

public class GetFeed : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("feeds/{id:guid}", async (
            [FromRoute]    Guid id,
            [FromServices] IFeedRepository feedRepository,
            [FromServices] IPostRepository postRepository,
            [FromServices] ISubscriptionRepository subscriptionRepository,
            [FromServices] CancellationToken ct) =>
        {
            var feed = await feedRepository.GetByIdAsync(id, ct);

            if (feed is null) return Results.NotFound();

            var posts = await postRepository.GetByFeedIdAsync(id, ct: ct);
            var subscribers = await subscriptionRepository.GetByFeedIdAsync(id, ct);

            var response = new GetFeedResponse(new FeedDto(
                id, feed.Name, feed.Description, feed.Url, subscribers.Length, posts.Total));

            return Results.Ok(response);
        }).WithTags(Tags.Feeds);
    }
}