using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Presentation.Contracts.Requests.UserManagement;

public record LoginRequest(
    [Required] string Email, 
    [Required] string Password);