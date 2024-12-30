﻿using FluentValidation;
using RssAggregator.Application.Auth;
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
            throw new NoAccessException();
        }

        await validator.ValidateAndThrowAsync(request, ct);

        var isFeedExist = await storage.IsFeedExist(request.FeedId, ct);

        if (!isFeedExist)
        {
            throw new NotFoundException<Feed>(request.FeedId);
        }

        var isSubscribedSuccessful = await storage.CreateSubscription(
            identityProvider.Current.UserId, request.FeedId, ct);

        if (!isSubscribedSuccessful)
        {
            throw new NotFoundException<AppUser>(identityProvider.Current.UserId);
        }

        var response = new CreateSubscriptionResponse();
        return response;
    }
}