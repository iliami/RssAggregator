using System.Reflection;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Infrastructure.BackgroundJobs.SyncFeedsService;
using RssAggregator.Persistence;
using RssAggregator.Persistence.Repositories;
using RssAggregator.Presentation.Extensions;
using RssAggregator.Presentation.Middleware;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddDbContext<AppDbContext>()
    .AddScoped<IAppUserRepository, AppUserRepository>()
    .AddScoped<ISubscriptionRepository, SubscriptionRepository>()
    .AddScoped<IFeedRepository, FeedRepository>()
    .AddScoped<IPostRepository, PostRepository>()
    .AddHttpClient()
    .AddHostedService<SyncAllFeedsJob>()
    .AddMemoryCache()
    .AddHttpContextAccessor()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();
    // .AddAuthenticationJwtBearer(signingOptions =>
    // {
    //     var singingKey = builder.Configuration["JwtOptions:SecretKey"];
    //     signingOptions.SigningKey = singingKey;
    // })
    // .AddAuthorization()
    // .SwaggerDocument()
    // .AddFastEndpoints();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

var apiPrefix = app.MapGroup("api/");

app.MapEndpoints(apiPrefix);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app
//     .UseSwaggerGen()
//     .UseJwtRevocation<TokenBlacklistCheckerMiddleware>()
//     .UseAuthentication()
//     .UseAuthorization()
//     .UseFastEndpoints();

app.Run();



/*
var builder = WebApplication.CreateBuilder(args);
   
   // Add services to the container.
   // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
   builder.Services.AddEndpointsApiExplorer();
   builder.Services.AddSwaggerGen();
   
   var app = builder.Build();
   
   // Configure the HTTP request pipeline.
   if (app.Environment.IsDevelopment())
   {
       app.UseSwagger();
       app.UseSwaggerUI();
   }
   
   app.UseHttpsRedirection();
   
   var summaries = new[]
   {
       "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
   };
   
   app.MapGet("/weatherforecast", () =>
       {
           var forecast = Enumerable.Range(1, 5).Select(index =>
                   new WeatherForecast
                   (
                       DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                       Random.Shared.Next(-20, 55),
                       summaries[Random.Shared.Next(summaries.Length)]
                   ))
               .ToArray();
           return forecast;
       })
       .WithName("GetWeatherForecast")
       .WithOpenApi();
   
   app.Run();
   
   record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
   {
       public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
   }
*/