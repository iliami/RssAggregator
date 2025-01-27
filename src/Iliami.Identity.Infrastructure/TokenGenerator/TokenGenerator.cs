using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Iliami.Identity.Domain;
using Iliami.Identity.Domain.Options;
using Iliami.Identity.Domain.TokenGenerator;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Iliami.Identity.Infrastructure.TokenGenerator;

public class TokenGenerator(IOptions<JwtOptions> options) : ITokenGenerator
{
    private JwtOptions JwtOptions { get; } = options.Value;

    public TokenResponse GenerateToken(User user)
    {
        var accessToken = CreateAccessToken(user);
        var accessTokenExpiration = DateTime.UtcNow
            .AddMinutes(options.Value.AccessTokenValidityInMinutes);

        var refreshToken = CreateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow
            .AddMinutes(options.Value.RefreshTokenValidityInMinutes);

        var response = new TokenResponse(
            accessToken,
            refreshToken,
            accessTokenExpiration,
            refreshTokenExpiration);

        return response;
    }

    private string CreateAccessToken(User user)
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

public static class TokenGeneratorExtensions
{
    public static class ClaimTypes
    {
        public const string JwtId = JwtRegisteredClaimNames.Jti;
        public const string IssAt = JwtRegisteredClaimNames.Iat;
        public const string UserId = "userid";
        public const string Email = "email";
        public const string Role = System.Security.Claims.ClaimTypes.Role;
    }

    public static Claim[] CreateClaims(this User user) =>
    [
        new(ClaimTypes.UserId, user.Id.ToString()),
        new(ClaimTypes.Email, user.Email),
        new(ClaimTypes.Role, user.Role)
    ];

    public static void ThrowIfInvalidAccessToken(this ITokenGenerator tokenGenerator, JwtOptions options, string? token)
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