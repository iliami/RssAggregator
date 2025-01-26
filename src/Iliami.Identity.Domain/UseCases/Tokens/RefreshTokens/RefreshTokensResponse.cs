namespace Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;

public record RefreshTokensResponse(string AccessToken, string RefreshToken);