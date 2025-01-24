namespace Iliami.Identity.Domain.Exceptions;

public class UserAlreadyCreatedException(string email) 
    : Exception($"User with email {email} already created");