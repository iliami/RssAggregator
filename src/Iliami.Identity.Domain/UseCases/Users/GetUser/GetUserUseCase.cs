using FluentValidation;
using Iliami.Identity.Domain.Exceptions;
using Iliami.Identity.Domain.HashingHelpers;

namespace Iliami.Identity.Domain.UseCases.Users.GetUser;

public class GetUserUseCase(
    IGetUserStorage storage,
    IValidator<GetUserRequest> validator,
    IHashComparer hashComparer) : IGetUserUseCase
{
    public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var (success, user) = await storage.TryGetUser(request.Email, ct);
        if (!success)
        {
            throw new UserNotFoundException(request.Email);
        }

        var hashComparisonResult = hashComparer.CompareWithHash(user.Password, request.Password);
        if (!hashComparisonResult)
        {
            throw new InvalidPasswordException(request.Email);
        }

        var response = new GetUserResponse(user.Id, user.Role);
        return response;
    }
}