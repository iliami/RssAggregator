using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.DependencyInjection;
using RssAggregator.Application.Models.Params;
using RssAggregator.Infrastructure.BackgroundJobs.SyncAllFeedsJob;
using RssAggregator.Persistence;
using RssAggregator.Persistence.DependencyInjection;
using RssAggregator.Persistence.Repositories;
using RssAggregator.Presentation.Extensions;
using RssAggregator.Presentation.Middleware;
using RssAggregator.Presentation.Options;
using RssAggregator.Presentation.Services;
using RssAggregator.Presentation.Services.Abstractions;

var builder = WebApplication.CreateBuilder();

builder.Services
    .AddApplication()
    .AddPersistence()
    .AddDbContext<AppDbContext>()
    .AddScoped<IAppUserRepository, AppUserRepository>()
    .AddScoped<ISubscriptionRepository, SubscriptionRepository>()
    .AddScoped<IFeedRepository, FeedRepository>()
    .AddScoped<IPostRepository, PostRepository>()
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddHttpClient()
    .AddHostedService<SyncAllFeedsJob>()
    .AddMemoryCache()
    .AddHttpContextAccessor()
    .AddEndpointsApiExplorer();


// Auth
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwtOptions = builder.Configuration.GetRequiredSection("JwtOptions").Get<JwtOptions>();
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = jwtOptions!.GetSecurityKey()
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RssAggregator", Version = "v1" });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            []
        }
    });
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter<SortDirection>(JsonNamingPolicy.CamelCase));
});

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());



var app = builder.Build();

var apiPrefix = app.MapGroup("api/");

app.MapEndpoints(apiPrefix);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<JwtRevocationMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.Run();