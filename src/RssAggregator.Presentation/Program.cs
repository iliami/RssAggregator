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
    .AddInfrastructure(builder.Configuration)
    .AddAuth(builder.Configuration)
    .AddEndpoints(Assembly.GetExecutingAssembly())
    .AddLogging(builder.Configuration)
    .AddSwagger();

var app = builder.Build();

var apiPrefix = app.MapGroup("api/");

app.MapEndpoints(apiPrefix);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<IdentityMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();