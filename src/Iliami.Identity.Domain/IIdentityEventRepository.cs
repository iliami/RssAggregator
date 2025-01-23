namespace Iliami.Identity.Domain;

public interface IIdentityEventRepository
{
    Task AddEvent<TEntity>(TEntity entity, CancellationToken ct = default);
}