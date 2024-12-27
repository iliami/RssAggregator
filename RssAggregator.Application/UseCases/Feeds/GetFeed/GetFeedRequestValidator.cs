using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public class GetFeedRequestValidator : AbstractValidator<GetFeedRequest>
{
    public GetFeedRequestValidator()
    {
        RuleFor(r => r.FeedId)
            .NotEmpty().WithMessage("Feed Id cannot be empty");

        RuleFor(r => r.Specification)
            .NotNull().WithMessage("Specification cannot be null");
        // TODO: .SetValidator(new SpecificationValidator());
    }
}