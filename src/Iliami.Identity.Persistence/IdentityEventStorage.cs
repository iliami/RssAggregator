using System.Text.Json;
using Iliami.Identity.Domain;

namespace Iliami.Identity.Persistence;

public class IdentityEventStorage(
    DbContext dbContext, 
    IGuidFactory guidFactory, 
    IBusPublisher busPublisher) : IIdentityEventStorage
{
    public async Task PublishEvent(User user, CancellationToken ct = default)
    {
        var identity = new RssAggregator.BusModels.Identity
        {
            Id = user.Id
        };

        var identityEvent = new IdentityEvent
        {
            Id = guidFactory.CreateGuid(),
            EmittedAt = DateTime.UtcNow,
            ContentBlob = JsonSerializer.SerializeToUtf8Bytes(identity)
        };

        await dbContext.AddAsync(identityEvent, ct);
        await dbContext.SaveChangesAsync(ct);

        await busPublisher.PublishAsync(identityEvent, ct);
    }
}