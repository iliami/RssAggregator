using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.AddPostsInFeed;

public class AddPostsInFeedRequestValidator : AbstractValidator<AddPostsInFeedRequest>
{
    public AddPostsInFeedRequestValidator()
    {
        RuleFor(r => r.Feed)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Feed is required")
            .SetValidator(new FeedValidator());

        RuleFor(r => r.Posts)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("A collection of posts cannot be null")
            .NotEmpty().WithMessage("A collection of posts cannot be empty")
            .Must((request, posts) => 
                posts.All(post => post?.Feed.Id == request.Feed?.Id)).WithMessage("Not all posts have the same feed id");

        RuleForEach(r => r.Posts)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("A post in the collection of posts cannot be null")
            .SetValidator(new PostValidator());
    }
}