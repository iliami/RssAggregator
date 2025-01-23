using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Posts;

namespace RssAggregator.Persistence.Tests.Storages.Posts;

public class CreatePostStorageShould
{
    [Fact]
    public async Task ReturnCreatedPostId_WhenPostIsCreated()
    {
        var dbContext = new TestDbContext();
        var sut = new CreatePostStorage(dbContext, Substitute.For<IMemoryCache>());
        const string title = "Post title";
        const string description = "Post description";
        var categories = new[] { "Category 1", "Category 2" };
        var publishDate = new DateTime(2018, 9, 20);
        const string url = "https://www.example.com/20180920/1";
        var feedId = Guid.Parse("1B65997D-D7EC-42F9-BF2B-BEFA45F984B4");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Feed name",
            Url = "https://www.example.com",
        };
        await dbContext.Feeds.AddAsync(feed);
        await dbContext.SaveChangesAsync();

        var actual = await sut.CreatePost(
            title,
            description,
            categories,
            publishDate,
            url,
            feedId);

        actual.Should().NotBeEmpty();
        dbContext.Posts.Should().Contain(post => post.Url == url).And.HaveCount(1);
    }

    private static Category[] GenerateCategories(Feed feed, params string[] categoryNames)
    {
        return categoryNames
            .Select(name => new Category
            {
                Id = Guid.NewGuid(),
                Name = name,
                NormalizedName = name.ToLowerInvariant(),
                Feed = feed
            })
            .ToArray();
    }
}