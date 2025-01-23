using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public class GetFeedsRequestValidator : AbstractValidator<GetFeedsRequest>
{
    public GetFeedsRequestValidator()
    {
        RuleFor(r => r.Specification)
            .NotNull().WithMessage("Specification cannot be null.");
        // TODO: Specification validator
    }
}