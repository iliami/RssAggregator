namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public class GetPostsUseCase(IGetPostsStorage storage) : IGetPostsUseCase
{
    public async Task<GetPostsResponse> Handle(GetPostsRequest request, CancellationToken ct = default)
    {
        var posts = await storage.GetPosts(
            request.PaginationParams, 
            request.SortingParams, 
            request.FilterParams, ct);
        
        var response = new GetPostsResponse(posts);
        
        return response;
    }
}