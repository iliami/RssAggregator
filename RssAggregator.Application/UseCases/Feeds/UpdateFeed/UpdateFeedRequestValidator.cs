using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.UpdateFeed;

public class UpdateFeedRequestValidator : AbstractValidator<UpdateFeedRequest>
{
    public UpdateFeedRequestValidator()
    {
        RuleFor(r => r.Feed)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Feed cannot be null");
            // TODO: am i need a feed validator here???
    }
}