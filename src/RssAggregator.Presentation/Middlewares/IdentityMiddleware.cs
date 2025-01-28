using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Application.Identity;

namespace RssAggregator.Presentation.Middlewares;

public static class ClaimTypes
{
    public const string JwtId = JwtRegisteredClaimNames.Jti;
    public const string IssAt = JwtRegisteredClaimNames.Iat;
    public const string UserId = "userid";
    public const string Email = "email";
    public const string Role = System.Security.Claims.ClaimTypes.Role;
}

public class IdentityMiddleware(RequestDelegate next)
{
    private const string Bearer = "Bearer ";

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.GetEndpoint()?.Metadata.OfType<IAllowAnonymous>().Any() is null or true)
        {
            await next(context);
            return;
        }

        var userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.UserId)!);
        var userRole = context.User.FindFirstValue(ClaimTypes.Role) ?? "base_user";
        var identity = new Identity
        {
            UserId = userId,
            Role = userRole
        };

        var identityProvider = context.RequestServices.GetRequiredService<IIdentityProvider>();
        identityProvider.Current = identity;

        await next(context);
    }
}