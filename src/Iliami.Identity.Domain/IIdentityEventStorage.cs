namespace Iliami.Identity.Domain;

public interface IIdentityEventStorage : IStorage
{
    Task PublishEvent(User user, CancellationToken ct = default);
}