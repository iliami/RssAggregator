using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RssAggregator.Application.UseCases.Identity.CreateUser;

namespace RssAggregator.Infrastructure;

public class IdentityEventListener(ILogger<IdentityEventListener> logger, IServiceProvider serviceProvider) : BackgroundService
{
    const string QueueName = RssAggregator.BusModels.Identity.IdentityQueueName;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        var factory = new ConnectionFactory { HostName = "localhost" }; // TODO: RabbitMQ configuration

        var connection = await factory.CreateConnectionAsync(stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

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
                logger.LogInformation("{ListenerName} received data from queue {QueueName}", nameof(IdentityEventListener), QueueName);
            }
            await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(QueueName, false, consumer, stoppingToken);
    }
}
