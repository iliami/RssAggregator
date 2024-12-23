using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

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
        var request = new GetFeedRequest(feedId);
        _storage.TryGetFeed(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((true, feed));
        var expected = new GetFeedResponse(feed);

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public async Task ThrowException_WhenFeedIsNotFound()
    {
        var feedId = Guid.Parse("4E0EE846-3FB4-471F-BCF0-A2069EB25307");
        var request = new GetFeedRequest(feedId);
        _storage.TryGetFeed(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((false, null!));

        var actual = _sut.Invoking(s => s.Handle(request));

        await actual.Should().ThrowExactlyAsync<NotFoundException<Feed>>();
    }
}