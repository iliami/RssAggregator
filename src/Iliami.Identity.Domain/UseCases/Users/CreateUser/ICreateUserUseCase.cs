namespace Iliami.Identity.Domain.UseCases.Users.CreateUser;

public interface ICreateUserUseCase
{
    Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken ct = default);
}