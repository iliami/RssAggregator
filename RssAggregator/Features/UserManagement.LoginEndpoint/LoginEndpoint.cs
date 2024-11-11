using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Infrastructure;
using RssAggregator.Persistence;

namespace RssAggregator.Features.UserManagement.LoginEndpoint;

public class LoginEndpoint(AppDbContext DbContext) : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
    }

    public override async Task<LoginResponse> ExecuteAsync(LoginRequest req, CancellationToken ct)
    {
        var storedUser = await DbContext.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Email == req.Email, ct);
        if (storedUser is null)
        {
            ThrowError("There is no user with this email", StatusCodes.Status404NotFound);
        }
        
        var isCorrectPassword = storedUser.Password.IsEqualToHashOf(req.Password);
        if (!isCorrectPassword)
        {
            ThrowError("Invalid password", StatusCodes.Status400BadRequest);
        }

        var secretKey = Config["JwtOptions:SecretKey"];
        if (secretKey is null)
        {
            ThrowError("Cannot create token", StatusCodes.Status500InternalServerError);
        }
        
        var token = JwtBearer.CreateToken(options =>
        {
            options.SigningKey = secretKey;
            options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, storedUser.Id.ToString()));
            options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Name, storedUser.Email));
            options.User.Roles.Add(storedUser.Role);
            options.ExpireAt = DateTime.Now.AddHours(12);
        });

        var response = new LoginResponse(token, storedUser.Email);

        return response;
    }
}