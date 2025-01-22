using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Feeds.UpdateFeed;

public class UpdateFeedUseCaseShould
{
    private readonly IUpdateFeedStorage _storage;
    private readonly UpdateFeedUseCase _sut;

    public UpdateFeedUseCaseShould()
    {
        var validator = Substitute.For<IValidator<UpdateFeedRequest>>();
        _storage = Substitute.For<IUpdateFeedStorage>();

        _sut = new UpdateFeedUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnResponseWithFeedId_WhenFeedIsUpdated()
    {
        var feed = new Feed
        {
            Id = Guid.Parse("5C847FA5-83F5-4470-9CA8-020DABDFE720"),
            Name = "Name",
            Url = "https://example.com",
            LastFetchedAt = DateTimeOffset.Now.AddMinutes(-1)
        };
        var request = new UpdateFeedRequest(feed);
        _storage.TryUpdateFeed(Arg.Any<Feed>(), CancellationToken.None).Returns((true, feed.Id));
        var expected = new UpdateFeedResponse(feed.Id);

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThrowFeedNotFoundException_WhenFeedIsNotFound()
    {
        var feed = new Feed
        {
            Id = Guid.Parse("A34DCF87-7A88-41C0-80CB-67B1939C6FBA"),
            Name = "Fail",
            Url = "https://fail-example.com",
            LastFetchedAt = DateTimeOffset.Now.AddMinutes(-11)
        };
        var request = new UpdateFeedRequest(feed);
        _storage.TryUpdateFeed(Arg.Any<Feed>(), CancellationToken.None).Returns((false, feed.Id));

        var actual = _sut.Invoking(s => s.Handle(request));

        await actual.Should().ThrowExactlyAsync<FeedNotFoundException>();
    }
}