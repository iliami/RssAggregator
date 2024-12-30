using RssAggregator.Domain.Entities;
using RssAggregator.Presentation.Endpoints.Auth;

namespace RssAggregator.Presentation.Services;

public interface ITokenService
{
    TokenResponse GenerateToken(AppUser user);
}