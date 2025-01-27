using Iliami.Identity.Domain;
using Iliami.Identity.Domain.HashingHelpers;
using Iliami.Identity.Domain.Options;
using Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;
using Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;
using Iliami.Identity.Domain.UseCases.Tokens.RevokeTokens;
using Iliami.Identity.Domain.UseCases.Users;
using Iliami.Identity.Domain.UseCases.Users.CreateUser;
using Iliami.Identity.Domain.UseCases.Users.GetUser;
using Iliami.Identity.Infrastructure.MQProvider;
using Iliami.Identity.Infrastructure.Storages.Tokens;
using Iliami.Identity.Infrastructure.Storages.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iliami.Identity.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<RabbitMQOptions>(configuration.GetSection(nameof(RabbitMQOptions)))

            .AddMemoryCache()
            .AddDbContext<DbContext>()
            .AddSingleton<IUnitOfWork, UnitOfWork>()
            
            .AddScoped<IGuidFactory, GuidFactory>()
            .AddScoped<IChannelProvider, ChannelProvider>()
            .AddScoped<IBusPublisher, BusPublisher>()
            .AddScoped<IHashCreator, HashingHelper>()
            .AddScoped<IHashComparer, HashingHelper>()
            
            .AddScoped<IIdentityEventStorage, IdentityEventStorage>()
            .AddScoped<ICreateUserStorage, CreateUserStorage>()
            .AddScoped<IGetUserStorage, GetUserStorage>()
            .AddScoped<IGenerateTokensStorage, GenerateTokensStorage>()
            .AddScoped<IRefreshTokensStorage, RefreshTokensStorage>()
            .AddScoped<IRevokeTokensStorage, RevokeTokensStorage>();
}