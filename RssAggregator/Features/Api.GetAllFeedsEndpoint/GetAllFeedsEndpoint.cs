using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Persistence;

namespace RssAggregator.Features.Api.GetAllFeedsEndpoint;

public class GetAllFeedsEndpoint(AppDbContext DbContext) : EndpointWithoutRequest<GetAllFeedsResponse>
{
    public override void Configure()
    {
        Get("api/feeds");
    }

    public override async Task<GetAllFeedsResponse> ExecuteAsync(CancellationToken ct)
    {
        var feeds = await DbContext.Feeds.AsNoTracking()
            .Select(f => new FeedDto(f.Id, f.Name)).ToListAsync(ct);

        return new GetAllFeedsResponse(feeds);
    }
}