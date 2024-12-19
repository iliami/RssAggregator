namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public interface IGetPostsUseCase
{
    Task<GetPostsResponse> Handle(GetPostsRequest request, CancellationToken ct = default);
}