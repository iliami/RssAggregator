using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.KeySelectors;
using RssAggregator.Application.Repositories;
using RssAggregator.Application.UseCases.Categories.CreateCategory;
using RssAggregator.Application.UseCases.Categories.GetCategories;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Application.UseCases.Feeds.GetUserFeeds;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Application.UseCases.Posts.CreatePost;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;
using RssAggregator.Application.UseCases.Posts.GetUserPosts;
using RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;
using RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;
using RssAggregator.Persistence.KeySelectors;
using RssAggregator.Persistence.Repositories;
using RssAggregator.Persistence.Storages;
using RssAggregator.Persistence.Storages.Categories;
using RssAggregator.Persistence.Storages.Feeds;
using RssAggregator.Persistence.Storages.Posts;
using RssAggregator.Persistence.Storages.Subscriptions;

namespace RssAggregator.Persistence.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
        => services
            .AddDbContext<AppDbContext>()
            .AddScoped<IAppUserRepository, AppUserRepository>()
            .AddKeySelectors()
            .AddScoped<IGetPostStorage, GetPostStorage>()
            .AddScoped<IGetPostsStorage, GetPostsStorage>()
            .AddScoped<IGetUserPostsStorage, GetUserPostsStorage>()
            .AddScoped<IGetPostsFromFeedStorage, GetPostsFromFeedStorage>()
            .AddScoped<ICreatePostStorage, CreatePostStorage>()
            .AddScoped<IGetFeedStorage, GetFeedStorage>()
            .AddScoped<IGetFeedsStorage, GetFeedsStorage>()
            .AddScoped<IGetUserFeedsStorage, GetUserFeedsStorage>()
            .AddScoped<ICreateFeedStorage, CreateFeedStorage>()
            .AddScoped<IUpdateFeedStorage, UpdateFeedStorage>()
            .AddScoped<IGetCategoriesStorage, GetCategoriesStorage>()
            .AddScoped<ICreateCategoryStorage, CreateCategoryStorage>()
            .AddScoped<ICreateSubscriptionStorage, CreateSubscriptionStorage>()
            .AddScoped<IDeleteSubscriptionStorage, DeleteSubscriptionStorage>();

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
                var serviceType = assignedTypes.GetInterfaces()
                    .First(i => i.GetGenericTypeDefinition() == typeof(IKeySelector<>));
                services.AddScoped(serviceType, assignedTypes);
            });

        return services;
    }
}