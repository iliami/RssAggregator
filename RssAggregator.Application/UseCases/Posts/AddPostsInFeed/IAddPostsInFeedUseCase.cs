using FluentValidation;
using FluentValidation.Validators;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.AddPostsInFeed;

public interface IAddPostsInFeedUseCase
{
    Task<AddPostsInFeedResponse> Handle(AddPostsInFeedRequest request, CancellationToken ct = default);
}

public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
        RuleFor(p => p.Title);
        RuleFor(p => p.Description);
        RuleFor(p => p.Url);
        RuleFor(p => p.PublishDate);
        RuleFor(p => p.Feed).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("A feed cannot be null")
            .SetValidator(new FeedValidator());
    }
}

public class FeedValidator : AbstractValidator<Feed>
{
    public FeedValidator()
    {
        RuleFor(f => f.Name);
        RuleFor(f => f.Description);
        RuleFor(f => f.Url);
        RuleFor(f => f.LastFetchedAt);
        RuleFor(f => f.Posts)
            .NotNull().WithMessage("A collection of posts in the feed cannot be null");
        RuleFor(f => f.Subscribers)
            .NotNull().WithMessage("A collection of subscribers in the feed cannot be null");
    }
}