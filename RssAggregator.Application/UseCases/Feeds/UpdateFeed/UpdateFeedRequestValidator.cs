using FluentValidation;
using RssAggregator.Application.UseCases.Posts.AddPostsInFeed;

namespace RssAggregator.Application.UseCases.Feeds.UpdateFeed;

public class UpdateFeedRequestValidator : AbstractValidator<UpdateFeedRequest>
{
    public UpdateFeedRequestValidator()
    {
        RuleFor(r => r.Feed)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Feed cannot be null")
            .SetValidator(new FeedValidator());
    }
}