namespace Iliami.Identity.Domain.UseCases.Users.GetUser;

public interface IGetUserStorage
{
    Task<(bool success, User user)> TryGetUser(string email, CancellationToken ct = default);
}