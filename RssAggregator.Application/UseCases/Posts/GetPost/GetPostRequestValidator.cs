using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.GetPost;

public class GetPostRequestValidator : AbstractValidator<GetPostRequest>
{
    public GetPostRequestValidator()
    {
    }
}