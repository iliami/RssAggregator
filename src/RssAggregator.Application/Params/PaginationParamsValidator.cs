using FluentValidation;

namespace RssAggregator.Application.Params;

public class PaginationParamsValidator : AbstractValidator<PaginationParams>
{
    public PaginationParamsValidator()
    {
        RuleFor(pp => pp.Page).GreaterThan(0).WithMessage("The page number must be greater than 0");
        RuleFor(pp => pp.PageSize).GreaterThanOrEqualTo(0).WithMessage("The page size must not be negative");
    }
}