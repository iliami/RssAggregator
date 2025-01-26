using System.Security.Claims;
using Iliami.Identity.Domain.TokenGenerator;
using Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;
using Microsoft.AspNetCore.Mvc;
using RssAggregator.Presentation.Endpoints;

namespace Iliami.Identity.Presentation.Endpoints.Auth;

public class RefreshToken : IEndpoint
{
    public record Request(string RefreshToken);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/refresh-token", async (
            [FromBody] Request req,
            [FromServices] IRefreshTokensUseCase useCase,
            ClaimsPrincipal user,
            CancellationToken ct) =>
        {
            var userId = Guid.Parse(user.FindFirstValue(TokenGeneratorExtensions.ClaimTypes.UserId)!);
            var request = new RefreshTokensRequest(userId, req.RefreshToken);

            var response = await useCase.Handle(request, ct);
            return Results.Ok(response);
        }).RequireAuthorization().WithTags(EndpointsTags.Auth);
    }
}