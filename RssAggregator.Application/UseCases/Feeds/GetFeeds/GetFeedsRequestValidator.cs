using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public class GetFeedsRequestValidator<TProjection> : AbstractValidator<GetFeedsRequest<TProjection>>
    where TProjection : class
{
    public GetFeedsRequestValidator()
    {
        RuleFor(r => r.Specification)
            .NotNull().WithMessage("Specification cannot be null.");
        // TODO: Specification validator
    }
}