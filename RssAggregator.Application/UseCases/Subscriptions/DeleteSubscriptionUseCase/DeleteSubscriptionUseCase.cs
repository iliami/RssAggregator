using FluentValidation;
using RssAggregator.Application.Auth;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;

public class DeleteSubscriptionUseCase(
    IDeleteSubscriptionStorage storage,
    IValidator<DeleteSubscriptionRequest> validator,
    IIdentityProvider identityProvider)
    : IDeleteSubscriptionUseCase
{
    public async Task<DeleteSubscriptionResponse> Handle(
        DeleteSubscriptionRequest request,
        CancellationToken ct = default)
    {
        if (!identityProvider.Current.IsAuthenticated())
        {
            throw new NoAccessException();
        }

        await validator.ValidateAndThrowAsync(request, ct);

        var isFeedExist = await storage.IsFeedExist(request.FeedId, ct);

        if (!isFeedExist)
        {
            throw new NotFoundException<Feed>(request.FeedId);
        }

        var isUnsubscribedSuccessful = await storage.DeleteSubscription(
            identityProvider.Current.UserId, request.FeedId, ct);

        if (!isUnsubscribedSuccessful)
        {
            throw new NotFoundException<User>(identityProvider.Current.UserId);
        }

        var response = new DeleteSubscriptionResponse();
        return response;
    }
}