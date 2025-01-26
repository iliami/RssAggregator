namespace Iliami.Identity.Domain.UseCases.Tokens.RevokeTokens;

public interface IRevokeTokensUseCase
{
    Task<RevokeTokensResponse> Handle(RevokeTokensRequest request, CancellationToken ct = default);
}