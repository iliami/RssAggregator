using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;

public class GetPostsFromFeedRequestValidator : AbstractValidator<GetPostsFromFeedRequest>
{
    public GetPostsFromFeedRequestValidator()
    {
        RuleFor(r => r.FeedId)
            .NotEmpty().WithMessage("Feed is required");

        RuleFor(r => r.Specification)
            .NotNull().WithMessage("Specification cannot be null.");
    }
}