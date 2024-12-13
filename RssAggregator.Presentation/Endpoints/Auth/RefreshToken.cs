using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Extensions;
using RssAggregator.Presentation.Options;
using RssAggregator.Presentation.Services.Abstractions;

namespace RssAggregator.Presentation.Endpoints.Auth;

public record TokenRequest(string Email, string RefreshToken);

public record TokenResponse(
    string Email,
    string AccessToken,
    string RefreshToken,
    [property: JsonIgnore] DateTime AccessTokenExpiration,
    [property: JsonIgnore] DateTime RefreshTokenExpiration);

public class RefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/refresh-token", async (
            [FromBody]     TokenRequest request,
            [FromServices] IAppUserRepository appUserRepository,
            [FromServices] IOptions<JwtOptions> options,
            [FromServices] ITokenService tokenService,
            [FromServices] IMemoryCache memoryCache,
            CancellationToken ct) =>
        { 
            var userEmail = request.Email;
            
            var storedUser = await appUserRepository.GetByEmailAsync(userEmail, ct);
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
            
            tokenService.ThrowIfInvalidAccessToken(options.Value, tokens.AccessToken);

            var response = tokenService.GenerateToken(storedUser);

            memoryCache.Set(key, response, response.RefreshTokenExpiration);
            
            memoryCache.Remove(tokens.AccessToken);
            memoryCache.Set(response.AccessToken, "true", response.AccessTokenExpiration);
            
            return Results.Ok(response);
        }).WithTags(EndpointsTags.Auth);
    }
}