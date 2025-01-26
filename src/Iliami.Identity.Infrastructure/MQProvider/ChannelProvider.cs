using Iliami.Identity.Domain.Constants;
using RabbitMQ.Client;

namespace Iliami.Identity.Infrastructure.MQProvider;

public class ChannelProvider : IChannelProvider
{
    private static bool _isInitialized = false;
    private static readonly ConnectionFactory Factory = new() { HostName = "localhost" }; 
    
    public async Task<IChannel> GetChannelAsync(CancellationToken ct = default)
    {
        var connection = await Factory.CreateConnectionAsync(ct);
        var channel = await connection.CreateChannelAsync(cancellationToken: ct);

        if (!_isInitialized)
        {
            _isInitialized = true;

            await channel.ExchangeDeclareAsync(
                MQConstants.ExchangeName, 
                MQConstants.ExchangeType,
                true, 
                cancellationToken: ct);
            await channel.QueueDeclareAsync(
                MQConstants.QueueName,
                true,
                false,
                false,
                cancellationToken: ct);
            await channel.QueueBindAsync(
                MQConstants.QueueName,
                MQConstants.ExchangeName, 
                MQConstants.ExchangeToQueueBindRoutingKey, 
                cancellationToken: ct);
        }
        
        return channel;
    }
}