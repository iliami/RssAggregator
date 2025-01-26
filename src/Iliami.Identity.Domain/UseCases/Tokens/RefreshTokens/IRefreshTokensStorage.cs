namespace Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;

public interface IRefreshTokensStorage
{
    Task<(bool success, User user)> TryGetUser(Guid userId, CancellationToken ct = default);
}