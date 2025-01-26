using FluentValidation;
using Iliami.Identity.Domain.Exceptions;
using Iliami.Identity.Domain.TokenGenerator;
using Microsoft.Extensions.Caching.Memory;

namespace Iliami.Identity.Domain.UseCases.Tokens.RevokeTokens;

public class RevokeTokensUseCase(
    IRevokeTokensStorage storage, 
    IValidator<RevokeTokensRequest> validator, 
    IMemoryCache memoryCache) : IRevokeTokensUseCase
{
    public async Task<RevokeTokensResponse> Handle(RevokeTokensRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);
        
        var (success, user) = await storage.TryGetUser(request.UserId, ct);

        if (!success)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var key = $"userid-{request.UserId}";
        var cachedTokens = memoryCache.Get<TokenResponse>(key);

        if (cachedTokens is null)
        {
            throw new TokenExpiredException(user.Email);
        }

        memoryCache.Remove(key);
        memoryCache.Remove(cachedTokens.AccessToken);

        var response = new RevokeTokensResponse();

        return response;
    }
}