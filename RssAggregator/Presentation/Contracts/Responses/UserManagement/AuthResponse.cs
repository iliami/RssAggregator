using System.ComponentModel.DataAnnotations;
using FastEndpoints.Security;

namespace RssAggregator.Presentation.Contracts.Responses.UserManagement;

public class AuthResponse : TokenResponse
{
    public string Email { get; set; } = null!;
}