using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Presentation.Contracts.Responses.UserManagement;

public record RegisterResponse(
    [Required] string Id, 
    [Required] string Email);