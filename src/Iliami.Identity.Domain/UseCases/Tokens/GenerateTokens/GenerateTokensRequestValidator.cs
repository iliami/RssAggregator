using FluentValidation;

namespace Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;

public class GenerateTokensRequestValidator : AbstractValidator<GenerateTokensRequest>
{
    public GenerateTokensRequestValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty();

        RuleFor(r => r.Role)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty();
    }
}