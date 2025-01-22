using FluentValidation;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Posts.CreatePost;

public class CreatePostUseCase(ICreatePostStorage storage, IValidator<CreatePostRequest> validator) : ICreatePostUseCase
{
    public async Task<CreatePostResponse> Handle(CreatePostRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var isFeedExist = await storage.IsFeedExist(request.FeedId, ct);
        if (!isFeedExist)
        {
            throw new FeedNotFoundException(request.FeedId);
        }

        var postId = await storage.CreatePost(
            request.Title,
            request.Description,
            request.Categories,
            request.PublishDate,
            request.Url,
            request.FeedId, ct);

        var response = new CreatePostResponse(postId);

        return response;
    }
}