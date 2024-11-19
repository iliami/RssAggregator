using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Presentation.Contracts.Requests.UserManagement;

public record RegisterRequest(
    string Email, 
    string Password);