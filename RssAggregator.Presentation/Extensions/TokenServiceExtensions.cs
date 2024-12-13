using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RssAggregator.Domain.Entities;
using RssAggregator.Presentation.Options;
using RssAggregator.Presentation.Services.Abstractions;

namespace RssAggregator.Presentation.Extensions;

public static class TokenServiceExtensions
{ 
    public static class ClaimTypes
    {
        public const string JwtId = JwtRegisteredClaimNames.Jti;
        public const string IssAt = JwtRegisteredClaimNames.Iat;
        public const string UserId = "userid";
        public const string Email = "email";
    }
    
    public static Claim[] CreateClaims(this AppUser user) => 
    [
        new(ClaimTypes.UserId, user.Id.ToString()),
        new(ClaimTypes.Email, user.Email),
    ];

    public static SymmetricSecurityKey GetSecurityKey(this JwtOptions options)
        => new(Encoding.UTF8.GetBytes(options.SecretKey));
    
    public static SigningCredentials GetSigningCredentials(this JwtOptions options)
        => new(options.GetSecurityKey(), SecurityAlgorithms.HmacSha256);

    public static void ThrowIfInvalidAccessToken(this ITokenService tokenService, JwtOptions options, string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = options.GetSecurityKey(),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(
            token, 
            tokenValidationParameters, 
            out var securityToken);
        
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid access token");
        }
    }
}