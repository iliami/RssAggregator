using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.Extensions.Caching.Memory;

namespace RssAggregator.Presentation.Middleware;

public class TokenBlacklistCheckerMiddleware(RequestDelegate next, IMemoryCache memoryCache)
    : JwtRevocationMiddleware(next)
{
    protected override Task<bool> JwtTokenIsValidAsync(string jwtToken, CancellationToken ct)
    {
        return Task.FromResult(!memoryCache.TryGetValue(jwtToken, out _));
    }

    protected override async Task SendTokenRevokedResponseAsync(HttpContext ctx, CancellationToken ct)
    {
        await ctx.Response.SendStringAsync("This token is revoked", StatusCodes.Status401Unauthorized,
            cancellation: ct);
    }
}