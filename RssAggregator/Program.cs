using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Persistence;

var builder = WebApplication.CreateBuilder();
builder.Services
    .SwaggerDocument()
    .AddDbContext<AppDbContext>(options =>
    {
        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_DATABASE_CONNECTIONSTRING") ?? 
                               builder.Configuration.GetConnectionString("DevelopmentPostgres");
        options.UseNpgsql(connectionString);
    })
    .AddScoped<IAppDbContext, AppDbContext>()
    .AddAuthenticationJwtBearer(signingOptions =>
    {
        var singingKey = builder.Configuration["JwtOptions:SecretKey"];
        signingOptions.SigningKey = singingKey;
    })
    .AddAuthorization()
    .AddFastEndpoints();


var app = builder.Build();
app
    .UseSwaggerGen()
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints();

app.Run();