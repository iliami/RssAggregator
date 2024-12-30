namespace RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;

public interface IGetPostsFromFeedUseCase
{
    Task<GetPostsFromFeedResponse> Handle(GetPostsFromFeedRequest request, CancellationToken ct = default);
}