using RabbitMQ.Client;

namespace Iliami.Identity.Infrastructure.MQProvider;

public interface IChannelProvider
{
    Task<IChannel> GetChannelAsync(CancellationToken ct = default);
}