using Iliami.Identity.Domain;
using Iliami.Identity.Domain.Constants;
using Iliami.Identity.Domain.UseCases.Users.CreateUser;
using Microsoft.EntityFrameworkCore;

namespace Iliami.Identity.Infrastructure.Storages.Users;

public class CreateUserStorage(
    DbContext dbContext, 
    IGuidFactory guidFactory) : ICreateUserStorage
{
    public async Task<(bool success, User user)> TryCreateUser(
        string email, 
        string passwordHash, 
        CancellationToken ct = default)
    {
        var isUserExists = await dbContext.Users.AnyAsync(u => u.Email == email, ct);

        if (isUserExists)
        {
            return (false, null!);
        }

        var user = new User
        {
            Id = guidFactory.CreateGuid(),
            Role = RoleConstants.Base,
            Email = email,
            Password = passwordHash
        };

        await dbContext.Users.AddAsync(user, ct);
        await dbContext.SaveChangesAsync(ct);
        return (true, user);
    }
}