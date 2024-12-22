using FluentValidation;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public class GetPostsRequestValidator : AbstractValidator<GetPostsRequest>
{
    public GetPostsRequestValidator()
    {
        RuleFor(r => r.PaginationParams)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(new PaginationParamsValidator());
        
        RuleFor(r => r.SortingParams)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(new SortingParamsValidator());
        
        RuleFor(r => r.FilterParams)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(new PostFilterParamsValidator());
    }
}