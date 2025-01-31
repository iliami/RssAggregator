using Iliami.Identity.Domain.DependencyInjection;
using Iliami.Identity.Infrastructure.DependencyInjection;
using Iliami.Identity.Presentation.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDomain()
    .AddInfrastructure(builder.Configuration)
    .AddAuth(builder.Configuration)
    .AddEndpoints()
    .AddLogging(builder.Configuration)
    .AddSwagger();

var app = builder.Build();

app.MapEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();