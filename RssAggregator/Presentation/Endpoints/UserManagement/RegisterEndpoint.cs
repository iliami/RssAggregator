using FastEndpoints;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.UserManagement;

public record RegisterRequest(string Email, string Password);

public record RegisterResponse(string Id, string Email);

public class RegisterEndpoint(IAppUserRepository UserRepository) : Endpoint<RegisterRequest, RegisterResponse>
{
    public override void Configure()
    {
        Post("auth/register");
        AllowAnonymous();
    }

    public override async Task<RegisterResponse> ExecuteAsync(RegisterRequest req, CancellationToken ct)
    {
        var id = await UserRepository.AddAsync(
            req.Email, 
            req.Password.GetHash(), 
            "base_user", ct);
        
        var res = new RegisterResponse(id.ToString(), req.Email);
        
        return res;
    }
}