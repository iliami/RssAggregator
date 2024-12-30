using FluentValidation;
using RssAggregator.Application.Auth;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Posts.GetUserPosts;

public class GetUserPostsUseCase(
    IGetUserPostsStorage storage,
    IValidator<GetUserPostsRequest> validator,
    IIdentityProvider identityProvider) : IGetUserPostsUseCase
{
    public async Task<GetUserPostsResponse> Handle(GetUserPostsRequest request, CancellationToken ct = default)
    {
        if (!identityProvider.Current.IsAuthenticated())
        {
            throw new NoAccessException();
        }

        await validator.ValidateAndThrowAsync(request, ct);

        var posts = await storage.GetUserPosts(identityProvider.Current.UserId, request.Specification, ct);

        var response = new GetUserPostsResponse(posts);

        return response;
    }
}