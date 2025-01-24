using RabbitMQ.Client;

namespace Iliami.Identity.Persistence.MQProvider;

public interface IChannelProvider
{
    Task<IChannel> GetChannelAsync(CancellationToken ct = default);
}