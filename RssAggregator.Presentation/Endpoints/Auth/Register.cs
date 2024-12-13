using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Extensions;

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
            CancellationToken ct) =>
        {
            var id = await appUserRepository.AddAsync(
                request.Email,
                request.Password.GetHash(),
                "base_user", ct);

            var response = new RegisterResponse(id.ToString(), request.Email);

            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Auth);
    }
}