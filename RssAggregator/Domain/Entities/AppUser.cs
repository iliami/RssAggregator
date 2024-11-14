using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Domain.Entities;

public class AppUser
{
    public Guid Id { get; set; }
    [StringLength(32)]
    public required string Email { get; set; }
    [StringLength(128)]
    public required string Password { get; set; }
    [StringLength(16)]
    public required string Role { get; set; }

    public List<Subscription> Subscriptions { get; set; } = [];
    public List<Feed> Feeds { get; set; } = [];
}