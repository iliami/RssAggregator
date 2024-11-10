using FastEndpoints;

namespace RssAggregator.Features;

public class HealthEndpoint : Endpoint<HealthRequest, HealthResponse>
{
    public override void Configure()
    {
        Get("/api/health");
        AllowAnonymous();
    }

    public override Task<HealthResponse> ExecuteAsync(HealthRequest req, CancellationToken ct)
    {
        return Task.FromResult(new HealthResponse(req.Check.ToUpper()));
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
public record HealthRequest(string Check);

public record HealthResponse(string AllCaps);