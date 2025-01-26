using FluentValidation;

namespace Iliami.Identity.Domain.UseCases.Users.GetUser;

public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    public GetUserRequestValidator()
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