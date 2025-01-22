using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.GetPost;

public class GetPostRequestValidator : AbstractValidator<GetPostRequest>
{
    public GetPostRequestValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Post Id is required");
    }
}