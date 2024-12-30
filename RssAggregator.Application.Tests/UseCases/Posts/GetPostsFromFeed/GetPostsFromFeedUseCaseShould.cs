using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.Specifications;
using RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Posts.GetPostsFromFeed;

public class GetPostsFromFeedUseCaseShould
{
    private readonly GetPostsFromFeedUseCase _sut;
    private readonly IGetPostsFromFeedStorage _storage;

    private static readonly Feed Feed = new()
    {
        Id = Guid.Parse("98EE3FA5-41C9-405B-88F0-AE2D701E0D75"),
        Name = "Feed",
        Url = "https://feeds.com/import/1/rss.xml",
    };

    private class TestSpecification : Specification<Post>;

    public GetPostsFromFeedUseCaseShould()
    {
        var validator = Substitute.For<IValidator<GetPostsFromFeedRequest>>();
        _storage = Substitute.For<IGetPostsFromFeedStorage>();

        _sut = new GetPostsFromFeedUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnPosts_WhenAllGood()
    {
        var posts = GeneratePosts(10);
        var feedId = Feed.Id;
        var request = new GetPostsFromFeedRequest(feedId, new TestSpecification());
        _storage
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(true);
        _storage
            .GetPostsFromFeed(Arg.Any<Guid>(), Arg.Any<Specification<Post>>(), CancellationToken.None)
            .Returns(posts);
        var expected = new GetPostsFromFeedResponse(posts);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThrowNotFoundExceptionOfFeed_WhenFeedNotExists()
    {
        var feedId = Feed.Id;
        var request = new GetPostsFromFeedRequest(feedId, new TestSpecification());
        _storage
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(false);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NotFoundException<Feed>>();
        await _storage
            .Received(0)
            .GetPostsFromFeed(Arg.Any<Guid>(), Arg.Any<Specification<Post>>(), CancellationToken.None);
    }

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
}