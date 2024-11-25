using FastEndpoints;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetFeedPostEndpoint(IPostRepository PostRepository) : EndpointWithoutRequest<GetFeedPostResponse>
{
    public override void Configure()
    {
        Get("api/posts/{id:guid}");
    }

    public override async Task<GetFeedPostResponse> ExecuteAsync(CancellationToken ct)
    {
        var postId = Route<Guid>("id");
        
        var post = await PostRepository.GetByIdAsync(postId, ct);

        if (post is null)
        {
            ThrowError("Post not found", StatusCodes.Status404NotFound);
        }
        
        var res = new GetFeedPostResponse(
            post.Id,
            post.Title,
            post.Description,
            post.PublishDate,
            post.Url,
            post.FeedId);

        return res;
    }
}