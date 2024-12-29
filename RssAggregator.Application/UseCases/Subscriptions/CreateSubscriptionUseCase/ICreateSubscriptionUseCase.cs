using FluentValidation;
using RssAggregator.Application.Auth;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;

public interface ICreateSubscriptionUseCase
{
    Task<CreateSubscriptionResponse> Handle(CreateSubscriptionRequest request, CancellationToken ct = default);
}

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

public interface ICreateSubscriptionStorage
{
    Task<bool> IsFeedExist(
        Guid feedId, 
        CancellationToken ct = default);
    Task<bool> CreateSubscription(
        Guid userId, 
        Guid feedId, 
        CancellationToken ct = default);
}

public record CreateSubscriptionRequest(Guid FeedId);

public class CreateSubscriptionRequestValidator : AbstractValidator<CreateSubscriptionRequest>
{
    public CreateSubscriptionRequestValidator()
    {
        RuleFor(r => r.FeedId)
            .NotEmpty().WithMessage("Feed is required");
    }
}
public record CreateSubscriptionResponse();