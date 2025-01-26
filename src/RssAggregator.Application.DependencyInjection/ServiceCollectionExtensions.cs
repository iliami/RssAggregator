using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.Auth;
using RssAggregator.Application.UseCases.Categories.CreateCategory;
using RssAggregator.Application.UseCases.Categories.GetCategories;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Application.UseCases.Feeds.GetUserFeeds;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Application.UseCases.Identity.CreateUser;
using RssAggregator.Application.UseCases.Posts.CreatePost;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;
using RssAggregator.Application.UseCases.Posts.GetUserPosts;
using RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;
using RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;

namespace RssAggregator.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddScoped<IIdentityProvider, IdentityProvider>()
            .AddValidatorsFromAssemblyContaining<GetPostUseCase>()
            .AddScoped<ICreateUserUseCase, CreateUserUseCase>()
            .AddScoped<IGetPostUseCase, GetPostUseCase>()
            .AddScoped<IGetPostsUseCase, GetPostsUseCase>()
            .AddScoped<IGetUserPostsUseCase, GetUserPostsUseCase>()
            .AddScoped<IGetPostsFromFeedUseCase, GetPostsFromFeedUseCase>()
            .AddScoped<ICreatePostUseCase, CreatePostUseCase>()
            .AddScoped<IGetFeedUseCase, GetFeedUseCase>()
            .AddScoped<IGetFeedsUseCase, GetFeedsUseCase>()
            .AddScoped<IGetUserFeedsUseCase, GetUserFeedsUseCase>()
            .AddScoped<ICreateFeedUseCase, CreateFeedUseCase>()
            .AddScoped<IUpdateFeedUseCase, UpdateFeedUseCase>()
            .AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>()
            .AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>()
            .AddScoped<ICreateSubscriptionUseCase, CreateSubscriptionUseCase>()
            .AddScoped<IDeleteSubscriptionUseCase, DeleteSubscriptionUseCase>();
}