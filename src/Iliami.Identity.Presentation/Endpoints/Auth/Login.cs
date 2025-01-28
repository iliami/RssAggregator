using Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;
using Iliami.Identity.Domain.UseCases.Users.GetUser;
using Microsoft.AspNetCore.Mvc;

namespace Iliami.Identity.Presentation.Endpoints.Auth;

public class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/login", async (
            [FromBody] GetUserRequest request,
            [FromServices] IGetUserUseCase getUserUseCase,
            [FromServices] IGenerateTokensUseCase generateTokensUseCase,
            CancellationToken ct) =>
        {
            var (userId, role) = await getUserUseCase.Handle(request, ct);
            var generateTokensRequest = new GenerateTokensRequest(userId, role);
            var response = await generateTokensUseCase.Handle(generateTokensRequest, ct);

            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Auth);
    }
}