using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.AddPostsInFeed;

public class AddPostsInFeedUseCase(IAddPostsInFeedStorage storage, IValidator<AddPostsInFeedRequest> validator) : IAddPostsInFeedUseCase
{
    public async Task<AddPostsInFeedResponse> Handle(AddPostsInFeedRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var isFeedExists = await storage.IsFeedExists(request.Feed.Id, ct);

        if (!isFeedExists)
        {
            throw new Exception("Feed does not exist"); // TODO: add domain exception
        }

        await storage.AddPosts(request.Posts, ct);

        return new AddPostsInFeedResponse();
    }
}