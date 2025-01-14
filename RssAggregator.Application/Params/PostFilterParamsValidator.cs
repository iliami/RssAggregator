﻿using FluentValidation;

namespace RssAggregator.Application.Params;

public class PostFilterParamsValidator : AbstractValidator<PostFilterParams>
{
    public PostFilterParamsValidator()
    {
        RuleFor(pfp => pfp.Categories).NotNull().WithMessage("The list of categories cannot be null");
    }
}