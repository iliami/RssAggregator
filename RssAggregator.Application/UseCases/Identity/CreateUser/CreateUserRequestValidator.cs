using FluentValidation;

namespace RssAggregator.Application.UseCases.Identity.CreateUser;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Id cannot be empty");
    }
}
