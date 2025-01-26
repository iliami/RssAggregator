namespace Iliami.Identity.Domain.UseCases.Users;

public interface IIdentityEventStorage : IStorage
{
    Task PublishEvent(User user, CancellationToken ct = default);
}