using FluentAssertions;
using RssAggregator.Application;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Posts;

namespace RssAggregator.Persistence.Tests.Storages.Posts;

public class GetPostsFromFeedStorageShould
{
    [Fact]
    public async Task ReturnTrue_WhenFeedIsExists()
    {
        var dbContext = new TestDbContext();
        var sut = new GetPostsFromFeedStorage(dbContext);
        var feedId = Guid.Parse("5B9AD7AF-02A8-4CA1-A024-BFB2982BB56D");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Feed name",
            Description = "Feed description",
            Url = "https://www.example.com"
        };
        await dbContext.Feeds.AddAsync(feed);
        await dbContext.SaveChangesAsync();

        var actual = await sut.IsFeedExist(feedId);

        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnFalse_WhenFeedIsNotExists()
    {
        var dbContext = new TestDbContext();
        var sut = new GetPostsFromFeedStorage(dbContext);
        var feedId = Guid.Parse("591FD634-6031-4FC9-A832-174E80ABDC06");

        var actual = await sut.IsFeedExist(feedId);

        actual.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnPosts_ThatInFeed()
    {
        var dbContext = new TestDbContext();
        var sut = new GetPostsFromFeedStorage(dbContext);
        var feedId = Guid.Parse("96B9D96C-BCAE-471A-8D7B-D2BF6A7C1805");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Some feed name",
            Url = "http://www.somefeed.com",
            LastFetchedAt = DateTimeOffset.UtcNow.AddDays(-5)
        };
        var postsInFeed = GeneratePostsForFeed(10, feed);
        var postsNotInFeed = GeneratePosts(10);
        var posts = postsInFeed.Concat(postsNotInFeed).ToArray();
        await dbContext.Posts.AddRangeAsync(posts);
        await dbContext.SaveChangesAsync();

        var actual = await sut.GetPostsFromFeed(feedId, new TestSpecification(), CancellationToken.None);

        actual.Should().BeEquivalentTo(postsInFeed);
    }

    private class TestSpecification : Specification<Post>;

    private static Post[] GeneratePostsForFeed(int count, Feed feed)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Post
            {
                Id = Guid.NewGuid(),
                Title = $"Post {i}",
                Description = $"Content {i}",
                Url = $"https://example.com/{i}/import.xml",
                Feed = feed,
                Categories = []
            })
            .ToArray();
    }

    private static Post[] GeneratePosts(int count)
    {
        var feed = new Feed
        {
            Id = Guid.Parse("51F86213-22A4-4373-901A-31503478EF7D"),
            Name = "Feed",
            Url = "https://www.feed-ex.com",
            LastFetchedAt = DateTimeOffset.UtcNow.AddHours(-1)
        };
        return Enumerable.Range(0, count)
            .Select(i => new Post
            {
                Id = Guid.NewGuid(),
                Title = $"{i}",
                Description = $"Post content {i}",
                Url = $"https://www.feed-ex.com/{i}/import.xml",
                Feed = feed,
                Categories = []
            })
            .ToArray();
    }
}