using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.GetPost;

public class GetPostRequestValidator : AbstractValidator<GetPostRequest>
{
    public GetPostRequestValidator()
    {
        RuleFor(r => r.PostId)
            .NotEmpty().WithMessage("Post Id is required");

        RuleFor(r => r.Specification)
            .NotNull().WithMessage("Specification is required");
    }
}