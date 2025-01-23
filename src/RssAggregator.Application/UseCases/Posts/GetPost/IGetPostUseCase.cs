namespace RssAggregator.Application.UseCases.Posts.GetPost;

public interface IGetPostUseCase
{
    Task<GetPostResponse> Handle(GetPostRequest request, CancellationToken ct = default);
}