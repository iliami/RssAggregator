namespace Iliami.Identity.Domain.Exceptions;

public class TokenExpiredException(string email) : Exception($"Tokens for user with email {email} were expired");