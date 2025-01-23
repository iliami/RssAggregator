using Iliami.Identity.Domain;
using Iliami.Identity.Domain.HashingHelpers;
using Microsoft.AspNetCore.Mvc;
using RssAggregator.Presentation.Endpoints;

namespace Iliami.Identity.Presentation.Endpoints.Auth;

public record RegisterRequest(string Email, string Password);

public record RegisterResponse(string Id, string Email);

public class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/register", async (
            [FromBody] RegisterRequest request,
            [FromServices] IUserRepository appUserRepository,
            [FromServices] IIdentityEventRepository identityEventRepository,
            [FromServices] IHashCreator hashCreator,
            CancellationToken ct) =>
        {
            var id = await appUserRepository.AddAsync(
                request.Email,
                hashCreator.GetHash(request.Password),
                "base_user", ct);

            var response = new RegisterResponse(id.ToString(), request.Email);

            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Auth);
    }
}