using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.Abstractions.KeySelectors;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Application.UseCases.Posts.CreatePost;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Persistence.KeySelectors;
using RssAggregator.Persistence.Storages;

namespace RssAggregator.Persistence.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
        => services
            .AddKeySelectors()

            .AddScoped<IGetPostStorage, GetPostStorage>()
            .AddScoped<IGetPostsStorage, GetPostsStorage>()
            .AddScoped<ICreatePostStorage, CreatePostStorage>()

            .AddScoped<IGetFeedStorage, GetFeedStorage>()
            .AddScoped<IGetFeedsStorage, GetFeedsStorage>()
            .AddScoped<ICreateFeedStorage, CreateFeedStorage>()
            .AddScoped<IUpdateFeedStorage, UpdateFeedStorage>();

    private static IServiceCollection AddKeySelectors(this IServiceCollection services)
    {
        System.Reflection.Assembly.GetAssembly(typeof(FeedKeySelector))!
            .GetTypes()
            .Where(item => item.GetInterfaces()
                .Where(i => i.IsGenericType)
                .Any(i => i.GetGenericTypeDefinition() == typeof(IKeySelector<>)) && 
                           item is { IsAbstract: false, IsInterface: false })
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IKeySelector<>));
                services.AddScoped(serviceType, assignedTypes);
            });
        
        return services;
    }
}