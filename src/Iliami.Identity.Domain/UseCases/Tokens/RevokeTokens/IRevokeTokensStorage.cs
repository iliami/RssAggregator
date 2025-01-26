namespace Iliami.Identity.Domain.UseCases.Tokens.RevokeTokens;

public interface IRevokeTokensStorage
{
    Task<(bool success, User user)> TryGetUser(Guid userId, CancellationToken ct = default);
}