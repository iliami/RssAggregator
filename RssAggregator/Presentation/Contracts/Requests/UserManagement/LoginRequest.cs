using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Presentation.Contracts.Requests.UserManagement;

public record LoginRequest(
    string Email, 
    string Password);