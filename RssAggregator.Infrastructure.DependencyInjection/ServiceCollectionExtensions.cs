using Microsoft.Extensions.DependencyInjection;

namespace RssAggregator.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddHttpClient()
            .AddHostedService<SyncAllFeedsJob.SyncAllFeedsJob>();
}