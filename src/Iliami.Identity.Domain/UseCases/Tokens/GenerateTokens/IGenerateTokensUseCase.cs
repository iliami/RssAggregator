namespace Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;

public interface IGenerateTokensUseCase
{
    Task<GenerateTokensResponse> Handle(GenerateTokensRequest request, CancellationToken ct = default);
}