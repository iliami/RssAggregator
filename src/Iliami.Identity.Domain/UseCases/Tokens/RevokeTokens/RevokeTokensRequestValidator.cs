using FluentValidation;

namespace Iliami.Identity.Domain.UseCases.Tokens.RevokeTokens;

public class RevokeTokensRequestValidator : AbstractValidator<RevokeTokensRequest>
{
    public RevokeTokensRequestValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty();
    }
}