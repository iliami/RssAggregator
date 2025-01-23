using FluentValidation;

namespace RssAggregator.Application.UseCases.Categories.GetCategories;

public class GetCategoriesRequestValidator : AbstractValidator<GetCategoriesRequest>
{
    public GetCategoriesRequestValidator()
    {
        RuleFor(r => r.Specification)
            .NotNull().WithMessage("Specification cannot be null.");
        // TODO: .SetValidator(new SpecificationValidator());
    }
}