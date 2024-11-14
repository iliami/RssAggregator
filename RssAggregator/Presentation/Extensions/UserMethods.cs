using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace RssAggregator.Presentation.Extensions;

public static class UserMethods
{
    public static (Guid UserId, string UserName) ToIdNameTuple(this ClaimsPrincipal User)
    {
        var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name)!;
        
        return (userId, userName);
    }
}