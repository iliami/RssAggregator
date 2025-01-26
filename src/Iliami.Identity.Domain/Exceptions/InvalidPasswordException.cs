namespace Iliami.Identity.Domain.Exceptions;

public class InvalidPasswordException(string email) : Exception($"Invalid password for user with email {email}");