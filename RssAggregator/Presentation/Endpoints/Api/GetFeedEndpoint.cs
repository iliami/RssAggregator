using FastEndpoints;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetFeedEndpoint(
    IFeedRepository FeedRepository,
    ISubscriptionRepository SubscriptionRepository,
    IPostRepository PostRepository) : EndpointWithoutRequest<GetFeedResponse>
{
    public override void Configure()
    {
        Get("api/feeds/{id:guid}");
    }

    public override async Task<GetFeedResponse> ExecuteAsync(CancellationToken ct)
    {
        var feedId = Route<Guid>("id");

        var storedFeed = await FeedRepository.GetByIdAsync(feedId, ct);
        var subscribers = await SubscriptionRepository.GetByFeedIdAsync(feedId, ct);
        var posts = await PostRepository.GetByFeedIdAsync(feedId, ct: ct);
        if (storedFeed is null)
        {
            ThrowError("Feed not found", StatusCodes.Status404NotFound);
        }

        var res = new GetFeedResponse(
            storedFeed.Id,
            storedFeed.Name,
            storedFeed.Description,
            storedFeed.Url,
            subscribers.Count(),
            posts.Count());

        return res;
    }
}