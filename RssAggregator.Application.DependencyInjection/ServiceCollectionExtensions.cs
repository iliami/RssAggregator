using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Application.UseCases.Posts.GetPosts;

namespace RssAggregator.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddScoped<IGetPostUseCase, GetPostUseCase>()
            .AddScoped<IGetPostsUseCase, GetPostsUseCase>();
}