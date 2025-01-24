using Iliami.Identity.Domain.Exceptions;
using Iliami.Identity.Domain.HashingHelpers;

namespace Iliami.Identity.Domain.UseCases.Users.CreateUser;

public class CreateUserUseCase(
    IUnitOfWork unitOfWork,
    IHashCreator hashCreator) : ICreateUserUseCase
{
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken ct = default)
    {
        await using var scope = await unitOfWork.StartScope(ct);

        var storage = scope.GetStorage<ICreateUserStorage>();
        var identityEventStorage = scope.GetStorage<IIdentityEventStorage>();

        var passwordHash = hashCreator.GetHash(request.Password);
        var (success, user) = await storage.TryCreateUser(request.Email, passwordHash, ct);

        if (!success)
        {
            throw new UserAlreadyCreatedException(request.Email);
        }

        await identityEventStorage.PublishEvent(user, ct);

        var response = new CreateUserResponse(user.Id);

        await scope.Commit(ct);

        return response;
    }
}