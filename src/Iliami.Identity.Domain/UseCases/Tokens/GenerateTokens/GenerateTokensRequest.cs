namespace Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;

public record GenerateTokensRequest(Guid UserId, string Role);