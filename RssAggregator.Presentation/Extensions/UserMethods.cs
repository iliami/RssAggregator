using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace RssAggregator.Presentation.Extensions;

public static class UserMethods
{
    public static (Guid UserId, string UserEmail) ToIdEmailTuple(this ClaimsPrincipal User)
    {
        var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var userEmail = User.FindFirstValue(JwtRegisteredClaimNames.Email)!;

        return (userId, userEmail);
    }
}