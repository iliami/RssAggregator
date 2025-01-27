using Iliami.Identity.Domain.Constants;
using Iliami.Identity.Domain.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Iliami.Identity.Infrastructure.MQProvider;

public interface IChannelProvider
{
    Task<IChannel> GetChannelAsync(CancellationToken ct = default);
}

public class ChannelProvider(IOptions<RabbitMQOptions> options) : IChannelProvider
{
    private static bool _isInitialized = false;
    private readonly ConnectionFactory _factory = new()
    {
        HostName = options.Value.HostName,
        Port = options.Value.Port,
        UserName = options.Value.UserName,
        Password = options.Value.Password,
        VirtualHost = options.Value.VirtualHost,
    }; 
    
    public async Task<IChannel> GetChannelAsync(CancellationToken ct = default)
    {
        var connection = await _factory.CreateConnectionAsync(ct);
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