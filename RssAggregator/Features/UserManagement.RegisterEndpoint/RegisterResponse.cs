using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Features.UserManagement.RegisterEndpoint;

public record RegisterResponse(
    [Required] string Id, 
    [Required] string Email);