using System.Security.Claims;
using Iliami.Identity.Domain.TokenGenerator;
using Iliami.Identity.Domain.UseCases.Tokens.RevokeTokens;
using Microsoft.AspNetCore.Mvc;
using RssAggregator.Presentation.Endpoints;

namespace Iliami.Identity.Presentation.Endpoints.Auth;

public class RevokeToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/revoke", async (
            [FromServices] IRevokeTokensUseCase useCase,
            ClaimsPrincipal user,
            CancellationToken ct) =>
        {
            var userId = Guid.Parse(user.FindFirstValue(TokenGeneratorExtensions.ClaimTypes.UserId)!);
            var request = new RevokeTokensRequest(userId);
            await useCase.Handle(request, ct);
            return Results.NoContent();
        }).RequireAuthorization().WithTags(EndpointsTags.Auth);
    }
}