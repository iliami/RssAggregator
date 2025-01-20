namespace Iliami.Identity.Domain.TokenGenerator;

public interface ITokenGenerator
{
    TokenResponse GenerateToken(User user);
}