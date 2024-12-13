using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Extensions;
using RssAggregator.Presentation.Services.Abstractions;

namespace RssAggregator.Presentation.Endpoints.Auth;

public record LoginRequest(string Email, string Password);

public class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/login", async (
            [FromBody]     LoginRequest request,
            [FromServices] IAppUserRepository appUserRepository,
            [FromServices] ITokenService tokenService,
            [FromServices] IMemoryCache memoryCache,
            CancellationToken ct) =>
        { 
            var storedUser = await appUserRepository.GetByEmailAsync(request.Email, ct);
            if (storedUser is null)
            {
                return Results.NotFound("There is no user with this email");
            }

            var isCorrectPassword = storedUser.Password.IsEqualToHashOf(request.Password);
            if (!isCorrectPassword)
            {
                 return Results.BadRequest("Invalid password");
            }
    
            var response = tokenService.GenerateToken(storedUser);

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