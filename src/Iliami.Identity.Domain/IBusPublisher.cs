namespace Iliami.Identity.Domain;

public interface IBusPublisher
{
    Task PublishAsync<TEntity>(TEntity entity, CancellationToken ct = default);
}