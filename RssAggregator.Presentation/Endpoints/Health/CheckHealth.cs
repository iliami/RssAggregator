using Microsoft.AspNetCore.Mvc;

namespace RssAggregator.Presentation.Endpoints.Health;

public record CheckHealthRequest(string Check);

public record CheckHealthResponse(string AllCaps);

public class CheckHealth : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("health", (
            [AsParameters] CheckHealthRequest request) =>
        {
            var allCaps = request.Check?.ToUpper() ?? string.Empty;
            var response = new CheckHealthResponse(allCaps);
            return Results.Ok(response);
        }).WithTags("Health");
    }
}