namespace Iliami.Identity.Domain;

public class User
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}

public static class RoleConstants
{
    public const string Base = "base_user";
}