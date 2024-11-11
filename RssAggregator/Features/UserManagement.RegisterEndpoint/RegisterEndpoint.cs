using FastEndpoints;
using RssAggregator.Infrastructure;
using RssAggregator.Persistence;
using RssAggregator.Persistence.Entities;

namespace RssAggregator.Features.UserManagement.RegisterEndpoint;

public class RegisterEndpoint(AppDbContext DbContext) : Endpoint<RegisterRequest, RegisterResponse>
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
            Role = req.Role
        };
        
        await DbContext.AppUsers.AddAsync(newUser, ct);
        
        await DbContext.SaveChangesAsync(ct);

        var res = new RegisterResponse(newUser.Id.ToString(), newUser.Email);
        
        return res;
    }
}