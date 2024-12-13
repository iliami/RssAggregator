using RssAggregator.Domain.Entities;
using RssAggregator.Presentation.Endpoints.Auth;

namespace RssAggregator.Presentation.Services.Abstractions;

public interface ITokenService
{
    TokenResponse GenerateToken(AppUser user);
}