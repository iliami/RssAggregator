namespace RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;

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