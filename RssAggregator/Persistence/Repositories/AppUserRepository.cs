using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Repositories;

public class AppUserRepository(IAppDbContext DbContext) : IAppUserRepository
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

    public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await DbContext.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);
}