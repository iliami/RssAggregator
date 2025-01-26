using System.Diagnostics;

namespace Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;

public interface IRefreshTokensUseCase
{
    Task<RefreshTokensResponse> Handle(RefreshTokensRequest request, CancellationToken ct = default);
}