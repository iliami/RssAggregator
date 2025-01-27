using FluentValidation;
using Iliami.Identity.Domain.TokenGenerator;
using Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;
using Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;
using Iliami.Identity.Domain.UseCases.Tokens.RevokeTokens;
using Iliami.Identity.Domain.UseCases.Users.CreateUser;
using Iliami.Identity.Domain.UseCases.Users.GetUser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iliami.Identity.Domain.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
        => services
            .AddValidatorsFromAssemblyContaining<ICreateUserUseCase>()
            .AddScoped<ICreateUserUseCase, CreateUserUseCase>()
            .AddScoped<IGetUserUseCase, GetUserUseCase>()
            .AddScoped<IGenerateTokensUseCase, GenerateTokensUseCase>()
            .AddScoped<IRefreshTokensUseCase, RefreshTokensUseCase>()
            .AddScoped<IRevokeTokensUseCase, RevokeTokensUseCase>();
}