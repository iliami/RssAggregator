using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Repositories;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Repositories;

public class AppUserRepository(AppDbContext DbContext) : IAppUserRepository
{
    public async Task<Guid> AddAsync(string email, string passwordHash, string role, CancellationToken ct = default)
    {
        var user = new AppUser
        {
            Email = email,
            Password = passwordHash,
            Role = role
        };

        await DbContext.AppUsers.AddAsync(user, ct);
        await DbContext.SaveChangesAsync(ct);

        return user.Id;
    }

    public Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return DbContext.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}