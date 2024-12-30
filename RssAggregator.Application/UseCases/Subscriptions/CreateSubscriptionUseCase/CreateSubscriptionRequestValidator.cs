using FluentValidation;

namespace RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;

public class CreateSubscriptionRequestValidator : AbstractValidator<CreateSubscriptionRequest>
{
    public CreateSubscriptionRequestValidator()
    {
        RuleFor(r => r.FeedId)
            .NotEmpty().WithMessage("Feed is required");
    }
}