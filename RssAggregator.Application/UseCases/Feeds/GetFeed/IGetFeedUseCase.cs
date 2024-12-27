namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public interface IGetFeedUseCase
{
    Task<GetFeedResponse> Handle(GetFeedRequest request, CancellationToken ct = default);
}