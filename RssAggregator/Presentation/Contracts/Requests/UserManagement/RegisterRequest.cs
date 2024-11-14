using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Presentation.Contracts.Requests.UserManagement;

public record RegisterRequest(
    [Required] string Email, 
    [Required] string Password, 
    [Required] string Role);