using System.Text.Json;
using Iliami.Identity.Domain.Constants;
using RabbitMQ.Client;

namespace Iliami.Identity.Infrastructure.MQProvider;

public interface IBusPublisher
{
    Task PublishAsync<TEntity>(TEntity entity, CancellationToken ct = default);
}

public class BusPublisher(IChannelProvider channelProvider) : IBusPublisher
{
    public async Task PublishAsync<TEntity>(TEntity entity, CancellationToken ct = default)
    {
        var channel = await channelProvider.GetChannelAsync(ct);
        var body = JsonSerializer.SerializeToUtf8Bytes(entity);

        await channel.BasicPublishAsync(
            MQConstants.ExchangeName, 
            MQConstants.ExchangeRoutingKey,
            body, 
            ct);
    }
}