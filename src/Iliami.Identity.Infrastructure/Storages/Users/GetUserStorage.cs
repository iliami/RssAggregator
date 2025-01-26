using Iliami.Identity.Domain;
using Iliami.Identity.Domain.UseCases.Users.GetUser;
using Microsoft.EntityFrameworkCore;

namespace Iliami.Identity.Infrastructure.Storages.Users;

public class GetUserStorage(DbContext dbContext) :  IGetUserStorage
{
    public async Task<(bool success, User user)> TryGetUser(string email, CancellationToken ct = default)
    {
        var user = await dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);
        return (user is null) ? 
            (false, null!) : 
            (true, user);
    }
}