using FastEndpoints;
using RssAggregator.Application.Abstractions;
using RssAggregator.Domain.Entities;
using RssAggregator.Presentation.Contracts.Requests.Admin;

namespace RssAggregator.Presentation.Endpoints.Admin;

public class CreateFeedEndpoint(IAppDbContext DbContext) : Endpoint<CreateFeedRequest>
{
    public override void Configure()
    {
        Post("admin/create-feed");
        Roles("admin"); 
    }

    public override async Task HandleAsync(CreateFeedRequest req, CancellationToken ct)
    {
        var feed = new Feed
        {
            Name = req.Name,
            Url = req.Url
        };
        
        await DbContext.Feeds.AddAsync(feed, ct);
        await DbContext.SaveChangesAsync(ct);
    }
}