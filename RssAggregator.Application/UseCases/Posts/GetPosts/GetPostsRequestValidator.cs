﻿using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public class GetPostsRequestValidator : AbstractValidator<GetPostsRequest>
{
    public GetPostsRequestValidator()
    {
        RuleFor(r => r.Specification)
            .NotNull().WithMessage("Specification cannot be null.");
    }
}