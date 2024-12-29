namespace RssAggregator.Application.Auth;

public interface IIdentity
{
    Guid UserId { get; }
    string Role { get; }
}

public class Identity : IIdentity
{
    public required Guid UserId { get; init; }
    public string Role { get; init; } = "base_user";
}

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity current) => current.UserId != Guid.Empty;
    public static bool IsAdminRole(this IIdentity current) => current.Role.Equals("admin", StringComparison.InvariantCultureIgnoreCase);
}