namespace Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;

public interface IGenerateTokensStorage
{
    Task<(bool success, User user)> TryGetUser(Guid userId, CancellationToken ct = default);
}