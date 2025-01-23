using Iliami.Identity.Domain;
using Iliami.Identity.Domain.HashingHelpers;
using Iliami.Identity.Domain.TokenGenerator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Presentation.Endpoints;

namespace Iliami.Identity.Presentation.Endpoints.Auth;

public record LoginRequest(string Email, string Password);

public class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/login", async (
            [FromBody] LoginRequest request,
            [FromServices] IUserRepository userRepository,
            [FromServices] ITokenGenerator tokenGenerator,
            [FromServices] IMemoryCache memoryCache,
            [FromServices] IHashComparer hashComparer,
            CancellationToken ct) =>
        {
            var storedUser = await userRepository.GetByEmailAsync(request.Email, ct);
            if (storedUser is null)
            {
                return Results.NotFound("There is no user with this email");
            }

            var isCorrectPassword = hashComparer.CompareWithHash(storedUser.Password, request.Password);
            if (!isCorrectPassword)
            {
                return Results.BadRequest("Invalid password");
            }

            var response = tokenGenerator.GenerateToken(storedUser);

            var key = $"userid-{storedUser.Id}";
            var success = memoryCache.TryGetValue<TokenResponse>(key, out var tokens);
            if (success)
            {
                memoryCache.Remove(tokens!.AccessToken);
            }

            memoryCache.Set(key, response, response.RefreshTokenExpiration);
            memoryCache.Set(response.AccessToken, "true", response.AccessTokenExpiration);

            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Auth);
    }
}