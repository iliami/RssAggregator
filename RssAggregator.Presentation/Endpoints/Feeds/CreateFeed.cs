using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public record CreateFeedRequest(string Name, string Url);

public class CreateFeed : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("feeds", async (
                [FromBody] CreateFeedRequest request,
                [FromServices] IFeedRepository feedRepository,
                [FromServices] CancellationToken ct) =>
        {
            var feedId = await feedRepository.AddAsync(request.Name, request.Url, ct);
            
            return Results.Created($"feeds/{feedId}", feedId);
        }).WithTags(Tags.Feeds);
    }
}