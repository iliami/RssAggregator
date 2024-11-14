using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetFeedEndpoint(IAppDbContext DbContext) : EndpointWithoutRequest<GetFeedResponse>
{
    public override void Configure()
    {
        Get("api/feeds/{id:guid}");
    }

    public override async Task<GetFeedResponse> ExecuteAsync(CancellationToken ct)
    {
        var feedId = Route<Guid>("id");

        var storedFeed = await DbContext.Feeds
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == feedId, ct);
        
        if (storedFeed is null)
        {
            ThrowError("Feed not found", StatusCodes.Status404NotFound);
        }
        
        var res = new GetFeedResponse(
            storedFeed.Id, 
            storedFeed.Name, 
            storedFeed.Description, 
            storedFeed.Url);

        return res;
    }
}