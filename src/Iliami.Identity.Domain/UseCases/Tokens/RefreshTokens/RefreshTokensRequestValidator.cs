using FluentValidation;

namespace Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;

public class RefreshTokensRequestValidator : AbstractValidator<RefreshTokensRequest>
{
    public RefreshTokensRequestValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty();

        RuleFor(r => r.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(32);
    }
}