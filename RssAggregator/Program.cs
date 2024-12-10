using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Infrastructure.BackgroundJobs.SyncFeedsService;
using RssAggregator.Persistence;
using RssAggregator.Persistence.Repositories;
using RssAggregator.Presentation.Middleware;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddDbContext<AppDbContext>()
    .AddScoped<IAppDbContext, AppDbContext>()
    .AddScoped<IAppUserRepository, AppUserRepository>()
    .AddScoped<ISubscriptionRepository, SubscriptionRepository>()
    .AddScoped<IFeedRepository, FeedRepository>()
    .AddScoped<IPostRepository, PostRepository>()
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