using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Features.UserManagement.LoginEndpoint;

public record LoginResponse(
    [Required] string Token, 
    [Required] string Email);