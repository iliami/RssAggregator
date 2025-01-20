using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RssAggregator.Presentation.Options;

namespace RssAggregator.Presentation.ServiceCollectionExtensions;

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