using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Application.UseCases.Posts.AddPostsInFeed;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Persistence.Storages;

namespace RssAggregator.Persistence.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
        => services
            .AddScoped<IGetPostStorage, GetPostStorage>()
            .AddScoped<IGetPostsStorage, GetPostsStorage>()
            .AddScoped<IAddPostsInFeedStorage, AddPostsInFeedStorage>()
            
            .AddScoped<IGetFeedStorage, GetFeedStorage>()
            .AddScoped<IGetFeedsStorage, GetFeedsStorage>()
            .AddScoped<ICreateFeedStorage, CreateFeedStorage>()
            .AddScoped<IUpdateFeedStorage, UpdateFeedStorage>();
}