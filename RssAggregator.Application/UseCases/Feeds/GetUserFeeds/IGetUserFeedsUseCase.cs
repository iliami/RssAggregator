namespace RssAggregator.Application.UseCases.Feeds.GetUserFeeds;

public interface IGetUserFeedsUseCase
{
    Task<GetUserFeedsResponse> Handle(GetUserFeedsRequest request, CancellationToken ct = default);
}