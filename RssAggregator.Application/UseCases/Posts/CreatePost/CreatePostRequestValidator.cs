using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.CreatePost;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(r => r.Title)
            .NotNull().WithMessage("Title is required")
            .MaximumLength(1024).WithMessage("Title must not exceed 1024 characters");
        
        RuleFor(r => r.Description)
            .NotNull().WithMessage("Description is required")
            .MaximumLength(32768).WithMessage("Description must not exceed 32768 characters");

        RuleFor(r => r.Categories)
            .NotNull().WithMessage("Categories is required");
        RuleForEach(r => r.Categories)
            .NotNull().WithMessage("Category is required")
            .MaximumLength(64).WithMessage("Category must not exceed 64 characters");

        RuleFor(r => r.Url)
            .NotNull().WithMessage("Url is required")
            .Matches(@"^(https?:\/\/)?(?:www\.)?([a-zA-Z0-9]{2,}\.)+[a-zA-Z0-9]{2,}(?:[-a-zA-Z0-9()@:%_\+.~#?&\/=]*)$").WithMessage("Url must be a valid url")
            .MaximumLength(256).WithMessage("Url must not exceed 256 characters");

        RuleFor(r => r.PublishDate)
            .NotEmpty().WithMessage("Publish Date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Publish Date must be less than or equal to UtcNow");

        RuleFor(r => r.FeedId)
            .NotEmpty().WithMessage("Feed Id is required");
    }
}