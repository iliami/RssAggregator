using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Presentation.Contracts.Responses.UserManagement;

public record LoginResponse(
    [Required] string Token, 
    [Required] string Email);