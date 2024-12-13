using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RssAggregator.Domain.Entities;
using RssAggregator.Presentation.Endpoints.Auth;
using RssAggregator.Presentation.Extensions;
using RssAggregator.Presentation.Options;
using RssAggregator.Presentation.Services.Abstractions;

namespace RssAggregator.Presentation.Services;

public class TokenService(IOptions<JwtOptions> options) : ITokenService
{
    private JwtOptions JwtOptions { get; } = options.Value;
    
    public TokenResponse GenerateToken(AppUser user)
    {
        var accessToken = CreateAccessToken(user);
        var accessTokenExpiration = DateTime.UtcNow
            .AddMinutes(options.Value.AccessTokenValidityInMinutes);

        var refreshToken = CreateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow
            .AddMinutes(options.Value.RefreshTokenValidityInMinutes);
        
        var response = new TokenResponse(
            user.Email,
            accessToken,
            refreshToken,
            accessTokenExpiration,
            refreshTokenExpiration);
        
        return response;
    }
    
    private string CreateAccessToken(AppUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var expires = DateTime.UtcNow.AddMinutes(JwtOptions.AccessTokenValidityInMinutes);
        var claims = user.CreateClaims();
        var signingCredentials = JwtOptions.GetSigningCredentials();
        
        var token = new JwtSecurityToken(
            expires: expires,
            claims: claims,
            signingCredentials: signingCredentials 
        );
        
        return tokenHandler.WriteToken(token);
    }
    
    private static string CreateRefreshToken()
        => Guid.NewGuid().ToString("N");
}