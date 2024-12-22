using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.UseCases.Feeds.GetFeed;

namespace RssAggregator.Application.Tests.Feeds.GetFeed;

public class GetFeedUseCaseShould
{
    private readonly GetFeedUseCase _sut;
    private readonly IGetFeedStorage _storage;

    public GetFeedUseCaseShould()
    {
        var validator = new GetFeedRequestValidator();
        _storage = Substitute.For<IGetFeedStorage>();

        _sut = new GetFeedUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnResponse_WhenFeedIsFound()
    {
        var feedId = Guid.Parse("83128A9B-9A93-472F-874D-90807DDE475B");
        var feed = new FeedDto(
            feedId,
            "Test feed",
            "Test feed description",
            "https://www.example.com",
            0,
            0);
        _storage.TryGetFeed(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((true, feed));
        var expected = (true, feed);

        var actual = await _storage.TryGetFeed(feedId, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ThrowException_WhenFeedIsNotFound()
    {
        var feedId = Guid.Parse("83128A9B-9A93-472F-874D-90807DDE475B");
        var feed = new FeedDto(
            feedId,
            "Test feed",
            "Test feed description",
            "https://www.example.com",
            0,
            0);
        _storage.TryGetFeed(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((false, null!));
        var expected = new Exception($"Feed not found: {feedId}");
        
        var actual = _storage.TryGetFeed(feedId, CancellationToken.None);

        actual.Should().Throws(expected);
    }
}