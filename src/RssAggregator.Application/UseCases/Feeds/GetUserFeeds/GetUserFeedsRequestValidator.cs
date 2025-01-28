using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.GetUserFeeds;

public class GetUserFeedsRequestValidator : AbstractValidator<GetUserFeedsRequest>
{
    public GetUserFeedsRequestValidator()
    {
        RuleFor(r => r.Specification)
            .NotNull().WithMessage("Specification cannot be null.");
    }
}