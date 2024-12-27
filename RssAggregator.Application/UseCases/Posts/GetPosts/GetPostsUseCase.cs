using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public class GetPostsUseCase(IGetPostsStorage storage, IValidator<GetPostsRequest> validator) : IGetPostsUseCase
{
    public async Task<GetPostsResponse> Handle(GetPostsRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);
        
        var posts = await storage.GetPosts(request.Specification, ct);
        
        var response = new GetPostsResponse(posts);
        
        return response;
    }
}