namespace Iliami.Identity.Domain;

public interface IUserRepository
{
    Task<Guid> AddAsync(string email, string passwordHash, string role, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
}