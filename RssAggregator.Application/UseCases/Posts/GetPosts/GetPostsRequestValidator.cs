using FluentValidation;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public class GetPostsRequestValidator : AbstractValidator<GetPostsRequest>
{
    public GetPostsRequestValidator()
    {
        RuleFor(r => r.PaginationParams)
            .NotNull().SetValidator(new PaginationParamsValidator());
        
        RuleFor(r => r.SortingParams)
            .NotNull().SetValidator(new SortingParamsValidator());
        
        RuleFor(r => r.FilterParams)
            .NotNull().SetValidator(new PostFilterParamsValidator());
    }
}