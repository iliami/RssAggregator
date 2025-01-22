using FluentValidation;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;

public class GetPostsFromFeedUseCase(
    IGetPostsFromFeedStorage storage,
    IValidator<GetPostsFromFeedRequest> validator)
    : IGetPostsFromFeedUseCase
{
    public async Task<GetPostsFromFeedResponse> Handle(GetPostsFromFeedRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var isFeedExist = await storage.IsFeedExist(request.FeedId, ct);
        if (!isFeedExist)
        {
            throw new FeedNotFoundException(request.FeedId);
        }

        var posts = await storage.GetPostsFromFeed(request.FeedId, request.Specification, ct);

        var response = new GetPostsFromFeedResponse(posts);

        return response;
    }
}