namespace RssAggregator.Application.UseCases.Posts.GetPost;

public class GetPostUseCase(IGetPostStorage storage) : IGetPostUseCase
{
    public async Task<GetPostResponse> Handle(GetPostRequest request, CancellationToken ct = default)
    {
        var (success, post) = await storage.TryGetAsync(request.Id, ct);
        if (!success)
        {
            return GetPostResponse.Empty;
        }

        var response = new GetPostResponse(
            post!.Id,
            post.Title,
            post.Description,
            post.Categories.Select(c => c.Name).ToArray(),
            post.PublishDate,
            post.Url,
            post.Feed.Id
        );

        return response;
    }
}