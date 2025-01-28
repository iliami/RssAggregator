using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.GetUserPosts;

public class GetUserPostsRequestValidator : AbstractValidator<GetUserPostsRequest>
{
    public GetUserPostsRequestValidator()
    {
        RuleFor(r => r.Specification)
            .NotNull().WithMessage("Specification cannot be null.");
    }
}