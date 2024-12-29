using FluentValidation;

namespace RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;

public class DeleteSubscriptionRequestValidator : AbstractValidator<DeleteSubscriptionRequest>
{
    public DeleteSubscriptionRequestValidator()
    {
        RuleFor(r => r.FeedId)
            .NotEmpty().WithMessage("Feed is required");
    }
}