using FluentValidation;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public class GetFeedsRequestValidator : AbstractValidator<GetFeedsRequest>
{
    public GetFeedsRequestValidator()
    {
        RuleFor(x => x.PaginationParams)
            .NotNull().WithMessage("Pagination parameters are required.")
            .SetValidator(new PaginationParamsValidator());
        
        RuleFor(x => x.SortingParams)
            .NotNull().WithMessage("Soring parameters are required.")
            .SetValidator(new SortingParamsValidator());
    }
}