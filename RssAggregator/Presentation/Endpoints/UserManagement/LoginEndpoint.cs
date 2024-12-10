using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.UserManagement;

public record LoginRequest(string Email, string Password);

public class AuthResponse : TokenResponse
{
    public string Email { get; set; } = null!;
}

public class LoginEndpoint(IAppDbContext DbContext, IAppUserRepository UserRepository) : Endpoint<LoginRequest, AuthResponse>
{
    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
    }

    public override async Task<AuthResponse> ExecuteAsync(LoginRequest req, CancellationToken ct)
    {
        var storedUser = await UserRepository.GetByEmailAsync(req.Email, ct);
        if (storedUser is null)
        {
            ThrowError("There is no user with this email", StatusCodes.Status404NotFound);
        }
        
        var isCorrectPassword = storedUser.Password.IsEqualToHashOf(req.Password);
        if (!isCorrectPassword)
        {
            ThrowError("Invalid password", StatusCodes.Status400BadRequest);
        }
        
        var response = await CreateTokenWith<RefreshTokenEndpoint>(storedUser.Id.ToString(), userPrivileges =>
        {
            userPrivileges.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, storedUser.Id.ToString()));
            userPrivileges.Claims.Add(new Claim(JwtRegisteredClaimNames.Email, storedUser.Email));
            userPrivileges.Roles.Add(storedUser.Role);
        });
        response.Email = storedUser.Email;
        
        return response;
    }
}