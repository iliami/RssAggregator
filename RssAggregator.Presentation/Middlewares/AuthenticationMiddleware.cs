using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Application.Auth;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Middlewares;

public class AuthenticationMiddleware(RequestDelegate next, IMemoryCache memoryCache)
{
    private const string Bearer = "Bearer ";

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.GetEndpoint()?.Metadata.OfType<IAllowAnonymous>().Any() is null or true)
        {
            await next(context);
            return;
        }

        var authHeader = context.Request.Headers.Authorization;

        if (!string.IsNullOrEmpty(authHeader) && authHeader[0]!.StartsWith(Bearer))
        {
            var token = authHeader[0]![Bearer.Length..].Trim();

            if (!IsJwtTokenValid(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("This token is revoked", context.RequestAborted);
                return;
            }
        }

        var (userId, _) = context.User.ToIdEmailTuple();
        var identity = new Identity
        {
            UserId = userId,
            Role = "admin" // TODO: Role
        };

        var identityProvider = context.RequestServices.GetRequiredService<IIdentityProvider>();
        identityProvider.Current = identity;

        await next(context);
    }

    private bool IsJwtTokenValid(string jwtToken) => memoryCache.TryGetValue(jwtToken, out _);
    
}