using FluentValidation;
using RssAggregator.Application.Identity;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;

public class CreateSubscriptionUseCase(
    ICreateSubscriptionStorage storage,
    IValidator<CreateSubscriptionRequest> validator,
    IIdentityProvider identityProvider)
    : ICreateSubscriptionUseCase
{
    public async Task<CreateSubscriptionResponse> Handle(
        CreateSubscriptionRequest request,
        CancellationToken ct = default)
    {
        if (!identityProvider.Current.IsAuthenticated())
        {
            throw new NotAuthenticatedException();
        }

        await validator.ValidateAndThrowAsync(request, ct);

        var isFeedExist = await storage.IsFeedExist(request.FeedId, ct);

        if (!isFeedExist)
        {
            throw new FeedNotFoundException(request.FeedId);
        }

        var isSubscribedSuccessful = await storage.CreateSubscription(
            identityProvider.Current.UserId, request.FeedId, ct);

        if (!isSubscribedSuccessful)
        {
            throw new UserNotFoundException(identityProvider.Current.UserId);
        }

        var response = new CreateSubscriptionResponse();
        return response;
    }
}