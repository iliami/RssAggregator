using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.CreateFeed;

public class CreateFeedRequestValidator : AbstractValidator<CreateFeedRequest>
{
    public CreateFeedRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name cannot be null")
            .MaximumLength(128).WithMessage("Name cannot exceed 128 characters");

        RuleFor(x => x.Url)
            .NotNull().WithMessage("Url cannot be null")
            .NotEmpty().WithMessage("Url cannot be empty")
            .Matches(@"^(https?:\/\/)?(?:www\.)?([a-zA-Z0-9]{2,}\.)+[a-zA-Z0-9]{2,}(?:[-a-zA-Z0-9()@:%_\+.~#?&\/=]*)$").WithMessage("Url must be a valid url")
            .MaximumLength(256).WithMessage("Url cannot exceed 256 characters");
    }
}