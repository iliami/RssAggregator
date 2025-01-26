using FluentValidation;
using Iliami.Identity.Domain.Exceptions;
using Iliami.Identity.Domain.TokenGenerator;
using Microsoft.Extensions.Caching.Memory;

namespace Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;

public class RefreshTokensUseCase(
    IRefreshTokensStorage storage,
    IValidator<RefreshTokensRequest> validator,
    ITokenGenerator tokenGenerator,
    IMemoryCache memoryCache) : IRefreshTokensUseCase
{
    public async Task<RefreshTokensResponse> Handle(RefreshTokensRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var (success, user) = await storage.TryGetUser(request.UserId, ct);

        if (!success)
        {
            throw new UserNotFoundException(request.UserId);
        }
        
        var key = $"userid-{user.Id}";
        var cachedTokens = memoryCache.Get<TokenResponse>(key);

        if (cachedTokens is null)
        {
            throw new TokenExpiredException(user.Email);
        }

        if (cachedTokens.RefreshToken != request.RefreshToken)
        {
            throw new InvalidRefreshTokenException(user.Email);
        }

        var tokens = tokenGenerator.GenerateToken(user);

        memoryCache.Set(key, tokens, tokens.RefreshTokenExpiration);

        memoryCache.Remove(cachedTokens.AccessToken);
        memoryCache.Set(tokens.AccessToken, "true", tokens.AccessTokenExpiration);
        
        var response = new RefreshTokensResponse(tokens.AccessToken, tokens.RefreshToken);

        return response;
    }
}