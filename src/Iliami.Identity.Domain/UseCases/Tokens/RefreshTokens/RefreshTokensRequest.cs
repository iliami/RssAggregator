namespace Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;

public record RefreshTokensRequest(Guid UserId, string RefreshToken);