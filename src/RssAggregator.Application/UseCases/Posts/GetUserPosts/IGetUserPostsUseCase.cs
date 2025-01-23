namespace RssAggregator.Application.UseCases.Posts.GetUserPosts;

public interface IGetUserPostsUseCase
{
    Task<GetUserPostsResponse> Handle(GetUserPostsRequest request, CancellationToken ct = default);
}