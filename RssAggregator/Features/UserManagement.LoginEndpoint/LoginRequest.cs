using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Features.UserManagement.LoginEndpoint;

public record LoginRequest(
    [Required] string Email, 
    [Required] string Password);