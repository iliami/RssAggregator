using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.Extensions.Caching.Memory;

namespace RssAggregator.Presentation.Endpoints.Auth;

public class RefreshTokenRequest : TokenRequest
{
}

public class RefreshTokenEndpoint : RefreshTokenService<RefreshTokenRequest, AuthResponse>
{
    private readonly IMemoryCache _memoryCache;

    public RefreshTokenEndpoint(IConfiguration config, IMemoryCache memoryCache)
    {
        Setup(options =>
        {
            options.TokenSigningKey = config["JwtOptions:SecretKey"];
            options.AccessTokenValidity = TimeSpan.FromHours(1);
            options.RefreshTokenValidity = TimeSpan.FromDays(1);

            options.Endpoint("auth/refresh-token", delegate { });
        });

        _memoryCache = memoryCache;
    }

    public override Task PersistTokenAsync(AuthResponse response)
    {
        var key = $"userid-{response.UserId}";
        _memoryCache.Set(key, response, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    public override Task RefreshRequestValidationAsync(RefreshTokenRequest req)
    {
        var key = $"userid-{req.UserId}";

        var success = _memoryCache.TryGetValue<AuthResponse>(key, out var tokens);
        if (!success)
        {
            AddError("There's no token to refresh");
        }
        else
        {
            _memoryCache.Remove(key);
            if (!(req.UserId == tokens!.UserId && req.RefreshToken == tokens.RefreshToken))
                AddError(r => r.RefreshToken, "Refresh token is invalid");
            else
                _memoryCache.Set(tokens.AccessToken, "true", TimeSpan.FromDays(1));
        }

        return Task.CompletedTask;
    }

    public override Task SetRenewalPrivilegesAsync(RefreshTokenRequest request, UserPrivileges privileges)
    {
        return Task.CompletedTask;
    }
}