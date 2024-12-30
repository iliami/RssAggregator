using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Auth;
using RssAggregator.Application.Repositories;
using RssAggregator.Presentation.Services;

namespace RssAggregator.Presentation.Endpoints.Auth;

public record RegisterRequest(string Email, string Password);

public record RegisterResponse(string Id, string Email);

public class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/register", async (
            [FromBody] RegisterRequest request,
            [FromServices] IAppUserRepository appUserRepository,
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