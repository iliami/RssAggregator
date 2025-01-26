namespace Iliami.Identity.Domain.UseCases.Users.GetUser;

public interface IGetUserUseCase
{
    Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken ct = default);
}