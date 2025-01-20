using System.Reflection;
using RssAggregator.Application.DependencyInjection;
using RssAggregator.Infrastructure.DependencyInjection;
using RssAggregator.Persistence.DependencyInjection;
using RssAggregator.Presentation.Middlewares;
using RssAggregator.Presentation.ServiceCollectionExtensions;
using Serilog;
using Serilog.Filters;
using Serilog.Sinks.OpenTelemetry;

var builder = WebApplication.CreateBuilder();

builder.Services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .Enrich.WithProperty("Application", "RssAggregator")
    .Enrich.WithProperty("Environment", "ASPNETCORE_ENVIRONMENT")
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = "http://localhost:4317";
        options.Protocol = OtlpProtocol.Grpc;
        options.ResourceAttributes = new Dictionary<string, object>
        {
            ["service.name"] = "rssaggregator"
        };
    })
    .CreateLogger()));

builder.Services
    .AddApplication()
    .AddPersistence()
    .AddInfrastructure()
    .AddAuth(builder.Configuration)
    .AddEndpoints(Assembly.GetExecutingAssembly())
    .AddSwagger();

var app = builder.Build();

var apiPrefix = app.MapGroup("api/");

app.MapEndpoints(apiPrefix);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<IdentityMiddleware>();

app.Run();