namespace RssAggregator.Application.UseCases.Posts.CreatePost;

public interface ICreatePostUseCase
{
    Task<CreatePostResponse> Handle(CreatePostRequest request, CancellationToken ct = default);
}