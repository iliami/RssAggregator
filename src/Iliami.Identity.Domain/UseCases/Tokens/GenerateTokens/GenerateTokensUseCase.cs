using FluentValidation;
using Iliami.Identity.Domain.Exceptions;
using Iliami.Identity.Domain.TokenGenerator;
using Microsoft.Extensions.Caching.Memory;

namespace Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;

public class GenerateTokensUseCase(
    IGenerateTokensStorage storage, 
    IValidator<GenerateTokensRequest> validator,
    ITokenGenerator tokenGenerator,
    IMemoryCache memoryCache) : IGenerateTokensUseCase
{
    public async Task<GenerateTokensResponse> Handle(GenerateTokensRequest request, CancellationToken ct = default)
    {   
        await validator.ValidateAndThrowAsync(request, ct);

        var (success, user) = await storage.TryGetUser(request.UserId, ct);
        if (!success)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var tokens = tokenGenerator.GenerateToken(user);

        var key = $"userid-{user.Id}";
        var isGetTokensFromCache = memoryCache.TryGetValue<TokenResponse>(key, out var cachedTokens);
        if (isGetTokensFromCache)
        {
            memoryCache.Remove(cachedTokens!.AccessToken);
        }

        memoryCache.Set(key, tokens, tokens.RefreshTokenExpiration);
        memoryCache.Set(tokens.AccessToken, "true", tokens.AccessTokenExpiration);

        var response = new GenerateTokensResponse(tokens.AccessToken, tokens.RefreshToken);
        return response;
    }
}