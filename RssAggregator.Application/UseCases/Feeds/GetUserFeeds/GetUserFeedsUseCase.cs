using FluentValidation;
using RssAggregator.Application.Auth;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Feeds.GetUserFeeds;

public class GetUserFeedsUseCase(
    IGetUserFeedsStorage storage,
    IValidator<GetUserFeedsRequest> validator,
    IIdentityProvider identityProvider) : IGetUserFeedsUseCase
{
    public async Task<GetUserFeedsResponse> Handle(GetUserFeedsRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        if (!identityProvider.Current.IsAuthenticated())
        {
            throw new NotAuthenticatedException();
        }

        var feeds = await storage.GetUserFeeds(identityProvider.Current.UserId, request.Specification, ct);

        var response = new GetUserFeedsResponse(feeds);

        return response;
    }
}