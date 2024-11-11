using FastEndpoints;
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
    });
    

var app = builder.Build();
app
    .UseFastEndpoints()
    .UseSwaggerGen();

app.Run();