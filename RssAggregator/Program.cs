using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Persistence;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument()
    .AddDbContext<AppDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Postgres");
        options.UseNpgsql(connectionString);
    })
    .AddAuthenticationJwtBearer(signingOptions =>
    {
        var singingKey = builder.Configuration["JwtOptions:SecretKey"];
        signingOptions.SigningKey = singingKey;
    })
    .AddAuthorization();


var app = builder.Build();
app
    .UseFastEndpoints()
    .UseSwaggerGen()
    .UseAuthentication()
    .UseAuthorization();

app.Run();