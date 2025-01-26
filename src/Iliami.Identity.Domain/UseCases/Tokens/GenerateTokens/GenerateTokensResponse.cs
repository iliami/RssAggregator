namespace Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;

public record GenerateTokensResponse(string AccessToken, string RefreshToken);