using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public class CreateFeed : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("feeds", async (
            [FromBody] CreateFeedRequest request,
            [FromServices] ICreateFeedUseCase useCase,
            ClaimsPrincipal user,
            CancellationToken ct) =>
        {
            var response = await useCase.Handle(request, ct);
            var feedId = response.FeedId;

            return Results.Created($"feeds/{feedId}", feedId);
        }).RequireAuthorization(builder => builder.RequireRole("admin")) .WithTags(EndpointsTags.Feeds);
    }
}