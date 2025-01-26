namespace Iliami.Identity.Domain.Exceptions;

public class UserNotFoundException: Exception
{
    public UserNotFoundException(string email) : base($"User with email {email} not found") {}
    public UserNotFoundException(Guid userId) : base($"User with id {userId} not found") {}
}