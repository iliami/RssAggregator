using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Application.UseCases.Posts.AddPostsInFeed;
using RssAggregator.Application.UseCases.Posts.CreatePost;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Application.UseCases.Posts.GetPosts;

namespace RssAggregator.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddValidatorsFromAssemblyContaining<GetPostUseCase>()
            .AddScoped<IGetPostUseCase, GetPostUseCase>()
            .AddScoped<IGetPostsUseCase, GetPostsUseCase>()
            .AddScoped<ICreatePostUseCase, CreatePostUseCase>()
            .AddScoped<IAddPostsInFeedUseCase, AddPostsInFeedUseCase>()
        
            .AddScoped<IGetFeedUseCase, GetFeedUseCase>()
            .AddScoped<IGetFeedsUseCase, GetFeedsUseCase>()
            .AddScoped<ICreateFeedUseCase, CreateFeedUseCase>()
            .AddScoped<IUpdateFeedUseCase, UpdateFeedUseCase>();
}