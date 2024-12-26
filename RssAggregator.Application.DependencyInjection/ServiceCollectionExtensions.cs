using System.Reflection.Metadata;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
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

            .AddScoped<IGetFeedUseCase, GetFeedUseCase>()
            .AddScoped(typeof(IGetFeedsUseCase<>), typeof(GetFeedsUseCase<>))
            .AddScoped<ICreateFeedUseCase, CreateFeedUseCase>()
            .AddScoped<IUpdateFeedUseCase, UpdateFeedUseCase>();


    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        var serviceType        = typeof(IValidator<>).MakeGenericType(typeof(GetFeedsRequest<>).GetGenericTypeDefinition());
        var implementationType = typeof(GetFeedsRequestValidator<>);

        return services.AddScoped(serviceType, implementationType);
    }
}