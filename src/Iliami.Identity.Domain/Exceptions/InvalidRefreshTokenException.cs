namespace Iliami.Identity.Domain.Exceptions;

public class InvalidRefreshTokenException(string email) : Exception($"Refresh token expired for user with email {email}");