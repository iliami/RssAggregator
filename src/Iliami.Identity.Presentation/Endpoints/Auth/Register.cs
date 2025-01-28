using Iliami.Identity.Domain.UseCases.Users.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace Iliami.Identity.Presentation.Endpoints.Auth;
public class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/register", async (
            [FromBody] CreateUserRequest request,
            [FromServices] ICreateUserUseCase useCase,
            CancellationToken ct) =>
        {
            var response = await useCase.Handle(request, ct);
            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Auth);
    }
}