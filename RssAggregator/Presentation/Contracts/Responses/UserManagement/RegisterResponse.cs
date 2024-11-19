using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Presentation.Contracts.Responses.UserManagement;

public record RegisterResponse(
    string Id, 
    string Email);