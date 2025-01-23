using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Iliami.Identity.Domain.Options;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenValidityInMinutes { get; set; }
    public int RefreshTokenValidityInMinutes { get; set; }
}

public static class JwtOptionsExtensions
{
    public static SymmetricSecurityKey GetSecurityKey(this JwtOptions options)
        => new(Encoding.UTF8.GetBytes(options.SecretKey));

    public static SigningCredentials GetSigningCredentials(this JwtOptions options)
        => new(options.GetSecurityKey(), SecurityAlgorithms.HmacSha256);
}