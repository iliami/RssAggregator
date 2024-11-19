using FastEndpoints;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Extensions;
using RssAggregator.Domain.Entities;
using RssAggregator.Presentation.Contracts.Requests.UserManagement;
using RssAggregator.Presentation.Contracts.Responses.UserManagement;

namespace RssAggregator.Presentation.Endpoints.UserManagement;

public class RegisterEndpoint(IAppDbContext DbContext) : Endpoint<RegisterRequest, RegisterResponse>
{
    public override void Configure()
    {
        Post("auth/register");
        AllowAnonymous();
    }

    public override async Task<RegisterResponse> ExecuteAsync(RegisterRequest req, CancellationToken ct)
    {
        var newUser = new AppUser
        {
            Email = req.Email,
            Password = req.Password.GetHash(),
            Role = "base_user"
        };
        
        await DbContext.AppUsers.AddAsync(newUser, ct);
        
        await DbContext.SaveChangesAsync(ct);

        var res = new RegisterResponse(newUser.Id.ToString(), newUser.Email);
        
        return res;
    }
}