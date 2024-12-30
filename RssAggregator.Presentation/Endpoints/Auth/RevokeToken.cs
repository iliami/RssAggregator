using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Presentation.Services;

namespace RssAggregator.Presentation.Endpoints.Auth;

public class RevokeToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/revoke", (
            [FromServices] IMemoryCache memoryCache,
            ClaimsPrincipal user) =>
        {
            var userId = Guid.Parse(user.FindFirstValue(TokenServiceExtensions.ClaimTypes.UserId)!);

            var key = $"userid-{userId}";
            var success = memoryCache.TryGetValue<TokenResponse>(key, out var tokens);

            if (!success)
            {
                return Results.NotFound();
            }

            memoryCache.Remove(key);
            memoryCache.Remove(tokens!.AccessToken);

            return Results.NoContent();
        }).RequireAuthorization().WithTags(EndpointsTags.Auth);
    }
}