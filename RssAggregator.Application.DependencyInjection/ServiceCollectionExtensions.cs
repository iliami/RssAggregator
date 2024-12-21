using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Application.UseCases.Posts.GetPosts;

namespace RssAggregator.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddValidatorsFromAssemblyContaining<GetPostUseCase>()
            .AddScoped<IGetPostUseCase, GetPostUseCase>()
            .AddScoped<IGetPostsUseCase, GetPostsUseCase>();
}