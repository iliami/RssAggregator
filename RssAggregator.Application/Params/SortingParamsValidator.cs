using FluentValidation;

namespace RssAggregator.Application.Params;

public class SortingParamsValidator : AbstractValidator<SortingParams>
{
    public SortingParamsValidator()
    {
        RuleFor(sp => sp.SortBy).NotNull().WithMessage("The field sort by cannot be null");
    }
}