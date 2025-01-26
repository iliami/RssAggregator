using Iliami.Identity.Domain;
using Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;
using Microsoft.EntityFrameworkCore;

namespace Iliami.Identity.Infrastructure.Storages.Tokens;

public class GenerateTokensStorage(DbContext dbContext) : IGenerateTokensStorage
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