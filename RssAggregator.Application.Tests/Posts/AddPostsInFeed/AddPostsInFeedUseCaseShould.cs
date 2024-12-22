using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RssAggregator.Application.UseCases.Posts.AddPostsInFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.Posts.AddPostsInFeed;

public class AddPostsInFeedUseCaseShould
{
    private readonly AddPostsInFeedUseCase _sut;
    private readonly IAddPostsInFeedStorage _storage;

    private static readonly Feed Feed = new Feed
    {
        Id = Guid.Parse("F6BB1027-5435-4E25-9AAA-8CCE6AAD0A6C"),
        Name = "Test feed",
        Description = "Test feed description",
        Url = "https://www.example.com",
        LastFetchedAt = DateTimeOffset.UtcNow
    };

    public AddPostsInFeedUseCaseShould()
    {
        var validator = Substitute.For<IValidator<AddPostsInFeedRequest>>();
        _storage = Substitute.For<IAddPostsInFeedStorage>();

        _sut = new AddPostsInFeedUseCase(_storage, validator);
    }

    [Fact]
    public async Task CallAddPostsOnStorage_WhenFeedExists()
    {
        var posts = GeneratePostsForFeed(10, Feed);
        var request = new AddPostsInFeedRequest(posts, Feed);
        _storage.IsFeedExists(Feed.Id).Returns(true);
        _storage
            .AddPosts(
                Arg.Any<Post[]>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await _sut.Handle(request);

        await _storage.Received(1).AddPosts(posts, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task NotCallAddPostsOnStorage_WhenFeedNotExists()
    {
        var posts = GeneratePostsForFeed(10, Feed);
        var request = new AddPostsInFeedRequest(posts, Feed);
        _storage.IsFeedExists(Feed.Id).Returns(false);
        var expectedException = new Exception("Feed does not exist");

        var actual = _sut.Handle(request);

        actual.Should().Throws(expectedException);
        await _storage.Received(0).AddPosts(posts, Arg.Any<CancellationToken>());
    }

    private static Post[] GeneratePostsForFeed(int count, Feed feed)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Post
            {
                Id = Guid.NewGuid(),
                Title = $"Post {i}",
                Description = $"Content {i}",
                Url = $"posts/{i}",
                PublishDate = DateTime.UtcNow.AddHours(-i),
                Categories = [],
                Feed = feed,
            })
            .ToArray();
    }
}