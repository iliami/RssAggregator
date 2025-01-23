namespace RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;

public interface IDeleteSubscriptionUseCase
{
    Task<DeleteSubscriptionResponse> Handle(DeleteSubscriptionRequest request, CancellationToken ct = default);
}