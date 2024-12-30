using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Application.UseCases.Posts.CreatePost;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages.Posts;

public class CreatePostStorage(AppDbContext dbContext, IMemoryCache memoryCache) : ICreatePostStorage
{
    public Task<bool> IsFeedExist(Guid feedId, CancellationToken ct = default)
        => dbContext.Feeds.AnyAsync(f => f.Id == feedId, ct);

    public async Task<Guid> CreatePost(
        string title,
        string description,
        string[] categories,
        DateTime publishDate,
        string url,
        Guid feedId,
        CancellationToken ct = default)
    {
        var feed = await dbContext.Feeds.FirstAsync(f => f.Id == feedId, ct);

        var postCategories = await GetCategoriesForPost(feed, categories, ct);

        var post = new Post()
        {
            Title = title,
            Description = description,
            Url = url,
            PublishDate = publishDate,
            Categories = postCategories,
            Feed = feed,
        };

        await dbContext.Posts.AddAsync(post, ct);
        await dbContext.SaveChangesAsync(ct);

        return post.Id;
    }

    private async Task<Category[]> GetCategoriesForPost(
        Feed feed,
        string[] categoriesNames,
        CancellationToken ct = default)
    {
        var feedCategories = await GetCategoriesFromCache(feed.Id, ct);

        var isAnyNewCategory = false;
        var categories = categoriesNames.Select(name =>
        {
            var storedCategory = feedCategories?.FirstOrDefault(x =>
                x.NormalizedName.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (storedCategory is not null)
            {
                return storedCategory;
            }

            isAnyNewCategory = true;
            return new Category
            {
                Name = name,
                NormalizedName = name.ToLowerInvariant(),
                Feed = feed
            };
        }).ToArray();

        if (!isAnyNewCategory)
        {
            return categories;
        }

        memoryCache.Remove($"feed_{feed.Id}_categories");
        dbContext.Categories.AttachRange(categories);
        await dbContext.SaveChangesAsync(ct);

        return categories;
    }

    private Task<Category[]?> GetCategoriesFromCache(
        Guid feedId,
        CancellationToken ct = default) =>
        memoryCache.GetOrCreateAsync(
            $"feed_{feedId}_categories",
            async builder =>
            {
                var categories = await dbContext.Categories
                    .Where(c => c.Feed.Id == feedId)
                    .ToArrayAsync(ct);
                builder.SetValue(categories);
                builder.SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddDays(1));
                return categories;
            });
}