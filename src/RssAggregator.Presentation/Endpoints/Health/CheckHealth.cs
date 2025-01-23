using Microsoft.AspNetCore.Mvc;

namespace RssAggregator.Presentation.Endpoints.Health;

public record CheckHealthResponse(string AllCaps);

public class CheckHealth : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("health", (
            [FromQuery] string check) =>
        {
            var allCaps = check.ToUpper();

            var response = new CheckHealthResponse(allCaps);

            return Results.Ok(response);
        }).AllowAnonymous().WithTags("Health");
    }
}