using FluentAssertions;
using RssAggregator.Application;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Posts;

namespace RssAggregator.Persistence.Tests.Storages.Posts;

public class GetPostStorageShould
{
    private static readonly Feed Feed = new()
    {
        Id = Guid.Parse("3B57928C-2CA6-4031-9FD2-72A69C70E757"), 
        Name = "Feed", 
        Url = "https://www.example.com"
    };

    [Fact]
    public async Task ReturnTrueAndPost_WhenPostIsExists()
    {
        var dbContext = new TestDbContext();
        var sut = new GetPostStorage(dbContext);
        var postId = Guid.Parse("583ED1CE-7664-4C44-B30D-AA6448956F1B");
        var post = new Post
        {
            Id = postId,
            Title = "Test feed",
            Description = "Test feed",
            Url = $"{Feed.Url}/{postId}",
            PublishDate = new DateTime(2020, 01, 01),
            Categories = [],
            Feed = Feed
        };
        await dbContext.Posts.AddAsync(post);
        await dbContext.SaveChangesAsync();

        var actual = await sut.TryGetPost(postId, new TestSpecification(), CancellationToken.None);

        actual.Should().BeEquivalentTo((true, post));
    }

    [Fact]
    public async Task ReturnFalseAndNull_WhenPostIsNotExists()
    {
        var dbContext = new TestDbContext();
        var sut = new GetPostStorage(dbContext);
        var postId = Guid.Parse("610D9738-CE23-4E47-B333-D86329D9C960");

        var actual = await sut.TryGetPost(postId, new TestSpecification(), CancellationToken.None);

        actual.Should().BeEquivalentTo<(bool, Post)>((false, null!));
    }

    private class TestSpecification : Specification<Post>;
}