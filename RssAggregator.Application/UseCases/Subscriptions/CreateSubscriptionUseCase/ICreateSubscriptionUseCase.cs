namespace RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;

public interface ICreateSubscriptionUseCase
{
    Task<CreateSubscriptionResponse> Handle(CreateSubscriptionRequest request, CancellationToken ct = default);
}