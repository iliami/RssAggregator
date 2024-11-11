using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Features.UserManagement.RegisterEndpoint;

public record RegisterRequest(
    [Required] string Email, 
    [Required] string Password, 
    [Required] string Role);