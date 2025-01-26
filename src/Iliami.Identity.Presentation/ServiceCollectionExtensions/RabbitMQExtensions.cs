using Iliami.Identity.Domain.Options;

namespace Iliami.Identity.Presentation.ServiceCollectionExtensions;

public static class RabbitMQExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<RabbitMQOptions>(configuration.GetSection(nameof(RabbitMQOptions)));
}