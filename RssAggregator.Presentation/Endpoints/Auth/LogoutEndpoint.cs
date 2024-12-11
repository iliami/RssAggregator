using FastEndpoints;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Auth;

public class LogoutEndpoint(IMemoryCache memoryCache) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("auth/logout");
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        var (userId, _) = User.ToIdEmailTuple();

        var key = $"userid-{userId}";
        var tokens = memoryCache.Get<AuthResponse>(key);

        if (tokens is not null)
        {
            memoryCache.Remove(key);
            memoryCache.Set(tokens.AccessToken, "true", TimeSpan.FromDays(1));
        }

        return Task.CompletedTask;
    }
}