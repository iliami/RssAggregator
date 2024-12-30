using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Repositories;

public interface IAppUserRepository
{
    Task<Guid> AddAsync(string email, string passwordHash, string role, CancellationToken ct = default);
    Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default);
}