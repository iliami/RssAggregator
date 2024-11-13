using FastEndpoints;
using RssAggregator.Persistence;
using RssAggregator.Persistence.Entities;

namespace RssAggregator.Features.Admin.CreateFeedEndpoint;

public class CreateFeedEndpoint(AppDbContext DbContext) : Endpoint<CreateFeedRequest>
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