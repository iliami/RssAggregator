using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace RssAggregator.Presentation.ServiceCollectionExtensions;

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

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var jwtOptions = configuration.GetRequiredSection("JwtOptions").Get<JwtOptions>();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtOptions!.GetSecurityKey()
                };
            });

        services.AddAuthorization();

        return services;
    }
}