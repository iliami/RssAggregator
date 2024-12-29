namespace RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;

public interface IDeleteSubscriptionStorage
{
    Task<bool> IsFeedExist(Guid feedId, CancellationToken ct = default);
    Task<bool> DeleteSubscription(Guid userId, Guid feedId, CancellationToken ct = default);
}