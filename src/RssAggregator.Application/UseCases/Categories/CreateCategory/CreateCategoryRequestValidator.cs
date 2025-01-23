using FluentValidation;

namespace RssAggregator.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.FeedId)
            .NotEmpty().WithMessage("FeedId is required");
    }
}