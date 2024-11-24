using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Infrastructure.BackgroundJobs.SyncFeedsService;
using RssAggregator.Persistence;
using RssAggregator.Presentation.Middleware;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddDbContext<AppDbContext>(options =>
    {
        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_RSSAGGREGATOR_DATABASE_CONNECTIONSTRING") ??
                               builder.Configuration.GetConnectionString("DevelopmentPostgres");
        options.UseNpgsql(connectionString);
    })
    .AddScoped<IAppDbContext, AppDbContext>()
    .AddHttpClient()
    .AddHostedService<SyncAllFeedsJob>()
    .AddMemoryCache()
    .AddAuthenticationJwtBearer(signingOptions =>
    {
        var singingKey = builder.Configuration["JwtOptions:SecretKey"];
        signingOptions.SigningKey = singingKey;
    })
    .AddAuthorization()
    .SwaggerDocument()
    .AddFastEndpoints();


var app = builder.Build();
app
    .UseSwaggerGen()
    .UseJwtRevocation<TokenBlacklistCheckerMiddleware>()
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints();

app.Run();