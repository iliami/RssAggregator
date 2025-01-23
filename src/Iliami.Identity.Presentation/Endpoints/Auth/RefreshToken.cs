using System.Text.Json.Serialization;
using Iliami.Identity.Domain;
using Iliami.Identity.Domain.Options;
using Iliami.Identity.Domain.TokenGenerator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RssAggregator.Presentation.Endpoints;

namespace Iliami.Identity.Presentation.Endpoints.Auth;

public record TokenRequest(string Email, string RefreshToken);

public class RefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/refresh-token", async (
            [FromBody] TokenRequest request,
            [FromServices] IUserRepository userRepository,
            [FromServices] IOptions<JwtOptions> options,
            [FromServices] ITokenGenerator tokenGenerator,
            [FromServices] IMemoryCache memoryCache,
            CancellationToken ct) =>
        {
            var userEmail = request.Email;

            var storedUser = await userRepository.GetByEmailAsync(userEmail, ct);
            if (storedUser is null)
            {
                return Results.NotFound("There is no user with this email");
            }

            var key = $"userid-{storedUser.Id}";
            var tokens = memoryCache.Get<TokenResponse>(key);
            if (tokens is null)
            {
                return Results.BadRequest("There is no token to refresh");
            }

            if (tokens.RefreshToken != request.RefreshToken)
            {
                return Results.BadRequest("Refresh tokens don't match");
            }

            tokenGenerator.ThrowIfInvalidAccessToken(options.Value, tokens.AccessToken);

            var response = tokenGenerator.GenerateToken(storedUser);

            memoryCache.Set(key, response, response.RefreshTokenExpiration);

            memoryCache.Remove(tokens.AccessToken);
            memoryCache.Set(response.AccessToken, "true", response.AccessTokenExpiration);

            return Results.Ok(response);
        }).WithTags(EndpointsTags.Auth);
    }
}