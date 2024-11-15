using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.DTO;
using RssAggregator.Presentation.DTO.FeedDto;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetAllFeedsEndpoint(IAppDbContext DbContext) : EndpointWithoutRequest<GetAllFeedsResponse>
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