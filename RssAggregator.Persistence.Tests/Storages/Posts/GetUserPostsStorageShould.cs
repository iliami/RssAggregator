using FluentAssertions;
using RssAggregator.Application;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Posts;

namespace RssAggregator.Persistence.Tests.Storages.Posts;

public class GetUserPostsStorageShould
{
    [Fact]
    public async Task ReturnFeeds_ThatUserIsSubscribedTo()
    {
        var dbContext = new TestDbContext();
        var sut = new GetUserPostsStorage(dbContext);
        var userId = Guid.Parse("C61AA63C-5DD0-4754-B8F0-561E1F5EFCEC");
        var user = new User { Id = userId };
        var userPosts = GeneratePostsForUser(10, user);
        var notUserPosts = GeneratePosts(10); 
        var posts = userPosts.Concat(notUserPosts).ToArray();
        await dbContext.Posts.AddRangeAsync(posts);
        await dbContext.SaveChangesAsync();

        var actual = await sut.GetUserPosts(userId, new TestSpecification());

        actual.Should().BeEquivalentTo(userPosts);
    }

    private class TestSpecification : Specification<Post>;
    
    private static Post[] GeneratePostsForUser(int count, User user)
    {
        var feed = new Feed
        {
            Id = Guid.Parse("F78EED24-BA40-4B08-A8FA-F0B104C8CC0C"),
            Name = "XFeed",
            Url = "https://www.x.com",
            LastFetchedAt = DateTimeOffset.UtcNow.AddMinutes(-14),
            Subscribers = [user]
        };
        return Enumerable.Range(0, count)
            .Select(i => new Post
            {
                Id = Guid.NewGuid(),
                Title = $"X POST {i}",
                Description = $"Content {i}",
                Url = $"https://x.com/{i}.xml",
                Feed = feed,
                Categories = []
            })
            .ToArray();
    }

    private static Post[] GeneratePosts(int count)
    {
        var feed = new Feed
        {
            Id = Guid.Parse("60C8AA78-4313-4F82-82C9-F7D0C3929C10"),
            Name = "Feed",
            Url = "https://www.feed.com",
            LastFetchedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Subscribers = []
        };
        return Enumerable.Range(0, count)
            .Select(i => new Post
            {
                Id = Guid.NewGuid(),
                Title = $"{i}",
                Description = $"Post {i}",
                Url = $"https://www.feed.com/import/{i}",
                Feed = feed,
                Categories = []
            })
            .ToArray();
    }
}