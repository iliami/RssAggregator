using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public class GetFeedUseCase(IGetFeedStorage storage) : IGetFeedUseCase
{
    public async Task<GetFeedResponse> Handle(GetFeedRequest request, CancellationToken ct = default)
    {
        var (success, feed) = await storage.TryGetFeed(request.Specification, ct);

        if (!success)
        {
            throw new FeedNotFoundException(request.FeedId);
        }

        var response = new GetFeedResponse(feed);

        return response;
    }
}