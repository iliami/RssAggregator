namespace Iliami.Identity.Domain.UseCases.Users.CreateUser;

public interface ICreateUserStorage : IStorage
{
    public Task<(bool success, User user)> TryCreateUser(
        string email, 
        string passwordHash, 
        CancellationToken ct = default);
}