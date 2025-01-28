using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RssAggregator.Application.UseCases.Identity.CreateUser;

namespace RssAggregator.Infrastructure;

public class RabbitMQOptions
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
}

public class IdentityEventListener(
    ILogger<IdentityEventListener> logger, 
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        var options = serviceProvider.GetRequiredService<IOptions<RabbitMQOptions>>();
        var factory = new ConnectionFactory
        {
            HostName = options.Value.HostName,
            Port = options.Value.Port,
            UserName = options.Value.UserName,
            Password = options.Value.Password,
            VirtualHost = options.Value.VirtualHost,
        };

        var connection = await factory.CreateConnectionAsync(stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(
            BusModels.Identity.ExchangeName, 
            BusModels.Identity.ExchangeType,
            true, 
            cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync(
            BusModels.Identity.QueueName,
            true,
            false,
            false,
            cancellationToken: stoppingToken);
        await channel.QueueBindAsync(
            BusModels.Identity.QueueName,
            BusModels.Identity.ExchangeName, 
            BusModels.Identity.ExchangeToQueueBindRoutingKey, 
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        var scope = serviceProvider.CreateAsyncScope();
        var createUserUseCase = scope.ServiceProvider.GetRequiredService<ICreateUserUseCase>();

        consumer.ReceivedAsync += async (_, args) =>
        {
            var identity = JsonSerializer.Deserialize<BusModels.Identity>(args.Body.Span);
            if (identity is not null)
            {
                var request = new CreateUserRequest(identity.Id);
                await createUserUseCase.Handle(request, stoppingToken);
                logger.LogInformation("{ListenerName} received data from queue {QueueName}", nameof(IdentityEventListener), BusModels.Identity.QueueName);
            }
            await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(BusModels.Identity.QueueName, false, consumer, stoppingToken);
    }
}
