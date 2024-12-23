using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;

namespace RssAggregator.Application.Tests.Feeds.CreateFeed;

public class CreateFeedUseCaseShould
{
    private readonly CreateFeedUseCase _sut;
    private readonly ICreateFeedStorage _storage;

    public CreateFeedUseCaseShould()
    {
        var validator = Substitute.For<IValidator<CreateFeedRequest>>();
        _storage = Substitute.For<ICreateFeedStorage>();
        
        _sut = new CreateFeedUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnCreatedFeedId_WhenFeedNotExists()
    {
        var feedId = Guid.Parse("DF31B440-DC70-46DB-BA04-9633E6E88980");
        var request = new CreateFeedRequest("Test name", "www.test-name.com");
        _storage
            .CreateFeed(
                Arg.Any<string>(),
                Arg.Any<string>(),
                CancellationToken.None)
            .Returns(feedId);
        var expected = new CreateFeedResponse(feedId);

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
        await _storage.Received(1).CreateFeed(request.Name, request.Url, CancellationToken.None);
    }

    [Fact]
    public async Task ReturnStoredFeedId_WhenFeedExists()
    {
        var feedId = Guid.Parse("73FC3772-87E5-4205-9E93-D3732BB19D2F");
        var request = new CreateFeedRequest("Feed name", "https://feed-name.com");
        _storage
            .CreateFeed(
                Arg.Any<string>(),
                Arg.Any<string>(),
                CancellationToken.None)
            .Returns(feedId);
        var expected = new CreateFeedResponse(feedId);

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
        await _storage.Received(1).CreateFeed(request.Name, request.Url, CancellationToken.None);
    }
}