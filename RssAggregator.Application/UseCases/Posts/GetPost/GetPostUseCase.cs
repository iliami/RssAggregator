using FluentValidation;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Posts.GetPost;

public class GetPostUseCase(IGetPostStorage storage, IValidator<GetPostRequest> validator) : IGetPostUseCase
{
    public async Task<GetPostResponse> Handle(GetPostRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var (success, post) = await storage.TryGetPost(request.Id, ct);
        if (!success)
        {
            throw new PostNotFoundException(request.Id);
        }

        var response = new GetPostResponse(post);

        return response;
    }
}