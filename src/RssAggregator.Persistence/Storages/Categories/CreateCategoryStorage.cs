using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.UseCases.Categories.CreateCategory;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages.Categories;

public class CreateCategoryStorage(AppDbContext dbContext) : ICreateCategoryStorage
{
    public Task<bool> IsFeedExist(Guid feedId, CancellationToken ct = default)
        => dbContext.Feeds.AnyAsync(f => f.Id == feedId, ct);

    public async Task<Guid> CreateCategory(string name, Guid feedId, CancellationToken ct = default)
    {
        var normalizedName = name.ToLowerInvariant();
        var storedCategory =
            await dbContext.Categories.FirstOrDefaultAsync(c => c.NormalizedName == normalizedName, ct);

        if (storedCategory is not null)
        {
            return storedCategory.Id;
        }

        var feed = await dbContext.Feeds.FirstAsync(f => f.Id == feedId, ct);

        var category = new Category
        {
            Name = name,
            NormalizedName = name.ToLowerInvariant(),
            Feed = feed
        };

        await dbContext.Categories.AddAsync(category, ct);
        await dbContext.SaveChangesAsync(ct);

        return category.Id;
    }
}