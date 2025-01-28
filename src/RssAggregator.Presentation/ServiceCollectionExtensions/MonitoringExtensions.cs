using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace RssAggregator.Presentation.ServiceCollectionExtensions;

public static class MonitoringExtensions
{
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var oTelCollectorOptions = configuration
                          .GetSection(nameof(OTelCollectorOptions))
                          .Get<OTelCollectorOptions>() 
                     ?? throw new ArgumentNullException(nameof(configuration));

        var oTelCollectorEndpoint = $"http://{oTelCollectorOptions.HostName}:{oTelCollectorOptions.Port}";
        var oTelCollectorProtocol = 
            oTelCollectorOptions.Protocol.Equals("http", StringComparison.InvariantCultureIgnoreCase) ? 
                OtlpProtocol.HttpProtobuf : 
                (oTelCollectorOptions.Protocol.Equals("grpc", StringComparison.InvariantCultureIgnoreCase) ? 
                    OtlpProtocol.Grpc : 
                    throw new ArgumentException(nameof(oTelCollectorOptions.Protocol)));

        services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .Enrich.WithProperty("Application", "RssAggregator")
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = oTelCollectorEndpoint;
                options.Protocol = oTelCollectorProtocol;
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = "rssaggregator"
                };
            })
            .CreateLogger()));
        
        return services;
    }
}

public class OTelCollectorOptions
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Protocol { get; set; } = string.Empty;
}