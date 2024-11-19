using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Presentation.Contracts.Responses.UserManagement;

public record LoginResponse(
    string Token, 
    string Email);