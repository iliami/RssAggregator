using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.Options;

namespace RssAggregator.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddHttpClient()
            .Configure<RabbitMQOptions>(configuration.GetSection(nameof(RabbitMQOptions)))
            .AddHostedService<SyncAllFeedsJob.SyncAllFeedsJob>()
            .AddHostedService<IdentityEventListener>();
}