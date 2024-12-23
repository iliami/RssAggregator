namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public interface IGetFeedsUseCase
{
    Task<GetFeedsResponse> Handle(GetFeedsRequest request, CancellationToken ct = default);    
}