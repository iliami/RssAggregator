using FluentValidation;

namespace RssAggregator.Application.UseCases.Posts.GetPost;

public class GetPostUseCase(IGetPostStorage storage, IValidator<GetPostRequest> validator) : IGetPostUseCase
{
    public async Task<GetPostResponse> Handle(GetPostRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);

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