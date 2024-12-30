using System.Reflection;
using RssAggregator.Application.DependencyInjection;
using RssAggregator.Infrastructure.DependencyInjection;
using RssAggregator.Persistence.DependencyInjection;
using RssAggregator.Presentation.Middlewares;
using RssAggregator.Presentation.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder();

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
app.UseMiddleware<AuthenticationMiddleware>();

app.Run();