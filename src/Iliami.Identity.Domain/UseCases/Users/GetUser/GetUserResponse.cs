namespace Iliami.Identity.Domain.UseCases.Users.GetUser;

public record GetUserResponse(Guid UserId, string Role);