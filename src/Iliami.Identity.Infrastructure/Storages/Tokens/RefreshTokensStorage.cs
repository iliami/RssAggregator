using Iliami.Identity.Domain;
using Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;
using Microsoft.EntityFrameworkCore;

namespace Iliami.Identity.Infrastructure.Storages.Tokens;

public class RefreshTokensStorage(DbContext dbContext) : IRefreshTokensStorage
{
    public async Task<(bool success, User user)> TryGetUser(Guid userId, CancellationToken ct = default)
    {
        var user = await dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, ct);
        return (user is null) ? 
            (false, null!) : 
            (true, user);
    }
}