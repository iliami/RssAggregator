using Iliami.Identity.Domain.DependencyInjection;
using Iliami.Identity.Infrastructure.DependencyInjection;
using Iliami.Identity.Presentation.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDomain()
    .AddInfrastructure()
    .AddAuth(builder.Configuration)
    .AddEndpoints()
    .AddSwagger();

var app = builder.Build();

app.MapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();