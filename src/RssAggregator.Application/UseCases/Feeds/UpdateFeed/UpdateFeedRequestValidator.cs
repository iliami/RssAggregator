using System.Data;
using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.UpdateFeed;

public class UpdateFeedRequestValidator : AbstractValidator<UpdateFeedRequest>
{
    public UpdateFeedRequestValidator()
    {
        RuleFor(r => r.Feed)
            .NotNull().WithMessage("Feed cannot be null");

        RuleFor(r => r.Feed.Id)
            .NotEmpty();

        RuleFor(r => r.Feed.Name)
            .NotNull()
            .MaximumLength(128);

        RuleFor(r => r.Feed.Url)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(r => r.Feed.Description)
            .NotNull()
            .NotEmpty()
            .MaximumLength(2048);
    }
}