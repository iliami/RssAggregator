using FluentValidation;

namespace Iliami.Identity.Domain.UseCases.Users.CreateUser;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(r => r.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .MaximumLength(32)
            .EmailAddress();

        RuleFor(r => r.Password)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Matches(@"^[^\s]{8,64}$");
    }
}