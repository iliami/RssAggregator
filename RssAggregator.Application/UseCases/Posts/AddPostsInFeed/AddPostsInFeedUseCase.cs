using FluentValidation;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Posts.AddPostsInFeed;

public class AddPostsInFeedUseCase(IAddPostsInFeedStorage storage, IValidator<AddPostsInFeedRequest> validator) : IAddPostsInFeedUseCase
{
    public async Task<AddPostsInFeedResponse> Handle(AddPostsInFeedRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var isFeedExists = await storage.IsFeedExists(request.Feed.Id, ct);

        if (!isFeedExists)
        {
            throw new NotFoundException<Feed>(request.Feed.Id);
        }

        await storage.AddPosts(request.Posts, ct);

        return new AddPostsInFeedResponse();
    }
}