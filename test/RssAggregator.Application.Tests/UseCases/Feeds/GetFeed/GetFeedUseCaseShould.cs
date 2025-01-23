using FluentAssertions;
using NSubstitute;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Feeds.GetFeed;

public class GetFeedUseCaseShould
{
    private readonly GetFeedUseCase _sut;
    private readonly IGetFeedStorage _storage;

    private class TestSpecification : Specification<Feed>;

    public GetFeedUseCaseShould()
    {
        _storage = Substitute.For<IGetFeedStorage>();

        _sut = new GetFeedUseCase(_storage);
    }

    [Fact]
    public async Task ReturnResponseWithFeed_WhenFeedIsFound()
    {
        var feedId = Guid.Parse("83128A9B-9A93-472F-874D-90807DDE475B");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Test feed",
            Description = "Test feed description",
            Url = "https://www.example.com"
        };
        var request = new GetFeedRequest(feedId, new TestSpecification());
        _storage.TryGetFeed(Arg.Any<Guid>(), Arg.Any<Specification<Feed>>(), Arg.Any<CancellationToken>()).Returns((true, feed));
        var expected = new GetFeedResponse(feed);

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThrowFeedNotFoundException_WhenFeedIsNotFound()
    {
        var feedId = Guid.Parse("4E0EE846-3FB4-471F-BCF0-A2069EB25307");
        var request = new GetFeedRequest(feedId, new TestSpecification());
        _storage.TryGetFeed(Arg.Any<Guid>(), Arg.Any<Specification<Feed>>(), Arg.Any<CancellationToken>()).Returns((false, null!));

        var actual = _sut.Invoking(s => s.Handle(request));

        await actual.Should().ThrowExactlyAsync<FeedNotFoundException>();
    }
}