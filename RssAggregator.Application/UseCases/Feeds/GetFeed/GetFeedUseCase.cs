using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public class GetFeedUseCase<TProjection>(IGetFeedStorage storage) : IGetFeedUseCase<TProjection>
where TProjection : class
{
    public async Task<GetFeedResponse<TProjection>> Handle(GetFeedRequest<TProjection> request, CancellationToken ct = default)
    {
        var (success, feed) = await storage.TryGetFeed(request.Specification, ct);

        if (!success)
        {
            throw new NotFoundException<Feed>(request.FeedId);
        }

        var response = new GetFeedResponse<TProjection>(feed);

        return response;
    }
}