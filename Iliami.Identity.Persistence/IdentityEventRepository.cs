using System.Text.Json;
using Iliami.Identity.Domain;

namespace Iliami.Identity.Persistence;

public class IdentityEventRepository(DbContext dbContext) : IIdentityEventRepository
{
    public async Task AddEvent<TEntity>(TEntity entity, CancellationToken ct = default)
    {
        var identityEvent = new IdentityEvent
        {
            EmittedAt = DateTime.UtcNow,
            ContentBlob = JsonSerializer.SerializeToUtf8Bytes(entity)
        };

        await dbContext.AddAsync(identityEvent, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}