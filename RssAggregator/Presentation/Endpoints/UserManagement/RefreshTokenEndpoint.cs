using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Presentation.Contracts.Requests.UserManagement;
using RssAggregator.Presentation.Contracts.Responses.UserManagement;

namespace RssAggregator.Presentation.Endpoints.UserManagement;

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
            
            options.Endpoint("auth/refresh-token", ep => { });
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
        
        var tokens = _memoryCache.Get<AuthResponse>(key);
        if (tokens is null)
        {
            AddError("There's no token to refresh");
        }
        else
        {
            _memoryCache.Remove(key);
            if (!(req.UserId == tokens.UserId && req.RefreshToken == tokens.RefreshToken))
            {
                AddError(r=>r.RefreshToken, "Refresh token is invalid");
            }
        }
        
        return Task.CompletedTask;
    }

    public override Task SetRenewalPrivilegesAsync(RefreshTokenRequest request, UserPrivileges privileges)
    {
        return Task.CompletedTask;
    }
}