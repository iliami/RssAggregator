using FluentAssertions;
using RssAggregator.Application;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Posts;

namespace RssAggregator.Persistence.Tests.Storages.Posts;

public class GetPostsStorageShould
{
    [Fact]
    public async Task ReturnPosts_WhenAllGood()
    {
        var dbContext = new TestDbContext();
        var sut = new GetPostsStorage(dbContext);
        var posts = GeneratePosts(20);
        await dbContext.Posts.AddRangeAsync(posts);
        await dbContext.SaveChangesAsync();

        var actual = await sut.GetPosts(new TestSpecification());

        actual.Should().BeEquivalentTo(posts);
    }

    private class TestSpecification : Specification<Post>;

    private static Post[] GeneratePosts(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Post
            {
                Id = Guid.NewGuid(),
                Title = $"Post {i}",
                Categories = [],
                PublishDate = DateTime.UtcNow,
                Description = $"Content {i}",
                Feed = Feed,
                Url = Feed.Url + $"/{i}"
            })
            .ToArray();
    }
    private static readonly Feed Feed = new()
    {
        Id = Guid.Parse("967679A0-CAFC-4D43-8BB9-86D5178B442F"),
        Name = "Feed",
        Description = "Some description",
        Url = "https://example.com",
        LastFetchedAt = DateTimeOffset.UtcNow.AddHours(-1)
    };
}