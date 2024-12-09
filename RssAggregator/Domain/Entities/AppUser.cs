namespace RssAggregator.Domain.Entities;

public class AppUser
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}