using FluentAssertions;
using RssAggregator.Application;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Categories;

namespace RssAggregator.Persistence.Tests.Storages.Categories;

public class GetCategoriesStorageShould
{
    private readonly TestDbContext _dbContext;
    private readonly GetCategoriesStorage _sut;

    public GetCategoriesStorageShould()
    {
        _dbContext = new TestDbContext();
        _sut = new GetCategoriesStorage(_dbContext);
    }

    [Fact]
    public async Task ReturnCategories_WhenAllGood()
    {
        await RestoreDb();
        var categories = CreateCategories(20);
        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();

        var actual = await _sut.GetCategories(new TestSpecification());

        actual.Should().BeEquivalentTo(categories);
    }

    private class TestSpecification : Specification<Category>;

    private async Task RestoreDb()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.EnsureCreatedAsync();
    }

    private Category[] CreateCategories(int count)
    {
        var feed = new Feed
        {
            Id = Guid.NewGuid(),
            Name = "Feed name",
            Description = "Feed description",
            Url = "http://example.com",
            LastFetchedAt = DateTimeOffset.UtcNow,
        };

        var categories = Enumerable.Range(0, count)
            .Select(i => new Category
            {
                Name = $"Name {i}",
                NormalizedName = $"Name {i}".ToLowerInvariant(),
                Feed = feed
            })
            .ToArray();

        return categories;
    }
}