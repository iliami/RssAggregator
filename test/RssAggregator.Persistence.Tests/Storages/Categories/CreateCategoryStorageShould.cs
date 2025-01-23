using FluentAssertions;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Categories;

namespace RssAggregator.Persistence.Tests.Storages.Categories;

public class CreateCategoryStorageShould
{
    private readonly TestDbContext _dbContext;
    private readonly CreateCategoryStorage _sut;

    public CreateCategoryStorageShould()
    {
        _dbContext = new TestDbContext();
        _sut = new CreateCategoryStorage(_dbContext);
    }

    [Fact]
    public async Task ReturnTrue_WhenFeedIsExists()
    {
        await RestoreDb();
        var feedId = Guid.Parse("8B3D9B5D-7658-49EF-A0E3-A1E710D2355E");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Feed name",
            Description = "Feed description",
            Url = "https://www.example.com"
        };
        await _dbContext.Feeds.AddAsync(feed);
        await _dbContext.SaveChangesAsync();

        var actual = await _sut.IsFeedExist(feedId);

        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnFalse_WhenFeedIsNotExists()
    {
        await RestoreDb();
        var feedId = Guid.Parse("33421C0F-DCBD-413D-95D4-2150C260D375");

        var actual = await _sut.IsFeedExist(feedId);

        actual.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnStoredCategoryId_WhenCategoryIsExists()
    {
        await RestoreDb();
        var feedId = Guid.Parse("44F50058-A723-4D0C-8F00-47E6029BC00F");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Feed name",
            Description = "Feed description",
            Url = "https://www.example.com"
        };
        var categoryId = Guid.Parse("75D99B44-F677-40ED-BE19-5A07581D9104");
        const string categoryName = "Category name";
        var category = new Category
        {
            Id = categoryId,
            Feed = feed,
            Name = categoryName,
            NormalizedName = categoryName.ToLowerInvariant()
        };
        await _dbContext.Feeds.AddAsync(feed);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var actual = await _sut.CreateCategory(categoryName, feedId);

        actual.Should().Be(categoryId);
    }

    [Fact]
    public async Task ReturnCreatedCategoryId_WhenCategoryIsNotExists()
    {
        await RestoreDb();
        var feedId = Guid.Parse("B5FF4C94-C00C-4357-B4C9-698AB5FB036D");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Feed name",
            Description = "Feed description",
            Url = "https://www.example.com"
        };
        const string categoryName = "Category name";
        await _dbContext.Feeds.AddAsync(feed);
        await _dbContext.SaveChangesAsync();

        var actual = await _sut.CreateCategory(categoryName, feedId, CancellationToken.None);

        actual.Should().NotBeEmpty();
    }

    private async Task RestoreDb()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.EnsureCreatedAsync();
    }
}