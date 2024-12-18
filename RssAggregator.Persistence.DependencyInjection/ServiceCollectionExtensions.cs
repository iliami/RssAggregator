using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Persistence.Storages;

namespace RssAggregator.Persistence.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
        => services.AddScoped<IGetPostStorage, GetPostStorage>();
}