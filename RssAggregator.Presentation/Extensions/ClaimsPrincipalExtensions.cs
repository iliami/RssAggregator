using System.Security.Claims;

namespace RssAggregator.Presentation.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static (Guid UserId, string UserEmail) ToIdEmailTuple(this ClaimsPrincipal user)
    {
        var userId = Guid.Parse(user.FindFirstValue(TokenServiceExtensions.ClaimTypes.UserId)!);
        var userEmail = user.FindFirstValue(TokenServiceExtensions.ClaimTypes.Email)!;

        return (userId, userEmail);
    }
}