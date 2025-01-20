using System.Text.Json;
using Iliami.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace Iliami.Identity.Persistence;

public class UserRepository : IUserRepository
{
    private const string QueueName = RssAggregator.BusModels.Identity.IdentityQueueName;
    private const string ExchangeName = "rssaggregator.direct";

    private static readonly IConnection Connection;

    private readonly DbContext _dbContext;
    private readonly IIdentityEventRepository _identityEventRepository;

    private readonly IChannel _channel;

    static UserRepository()
    {
        var factory = new ConnectionFactory { HostName = "localhost" }; // TODO: RabbitMQ configuration
        Connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
    }

    public UserRepository(DbContext dbContext, IIdentityEventRepository identityEventRepository)
    {
        _dbContext = dbContext;
        _identityEventRepository = identityEventRepository;
        _channel = Connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.ExchangeDeclareAsync(
            ExchangeName, 
            ExchangeType.Direct,
            true).GetAwaiter().GetResult();
        _channel.QueueDeclareAsync(queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null).GetAwaiter().GetResult();
        _channel.QueueBindAsync(
            QueueName,
            ExchangeName, 
            "identity").GetAwaiter().GetResult();
    }

    public async Task<Guid> AddAsync(string email, string passwordHash, string role, CancellationToken ct = default)
    {
        // TODO: transaction
        var user = new User
        {
            Email = email,
            Password = passwordHash,
            Role = role
        };

        await _dbContext.Users.AddAsync(user, ct);
        await _dbContext.SaveChangesAsync(ct);

        await PublishEvent(user, ct);

        return user.Id;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    private async Task PublishEvent(User user, CancellationToken ct = default)
    {
        var identity = new RssAggregator.BusModels.Identity
        {
            Id = user.Id
        };

        await _identityEventRepository.AddEvent(identity, ct);

        var body = JsonSerializer.SerializeToUtf8Bytes(identity);

        await _channel.BasicPublishAsync(
            ExchangeName,
            "identity",
            body,
            ct);
    }
}