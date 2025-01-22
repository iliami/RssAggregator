using FluentValidation;

namespace RssAggregator.Application.UseCases.Identity.CreateUser;

public class CreateUserUseCase(ICreateUserStorage storage, IValidator<CreateUserRequest> validator) : ICreateUserUseCase
{
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        await storage.CreateUser(request.Id, ct);
        
        return new CreateUserResponse();
    }
}
