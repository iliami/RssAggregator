namespace RssAggregator.Application.UseCases.Feeds.CreateFeed;

public interface ICreateFeedUseCase
{
    Task<CreateFeedResponse> Handle(CreateFeedRequest request, CancellationToken ct = default);
}