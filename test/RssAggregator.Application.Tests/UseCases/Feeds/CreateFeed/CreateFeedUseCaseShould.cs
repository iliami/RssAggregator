using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.Identity;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Feeds.CreateFeed;

public class CreateFeedUseCaseShould
{
    private readonly CreateFeedUseCase _sut;
    private readonly ICreateFeedStorage _storage;
    private readonly IIdentity _identity;

    public CreateFeedUseCaseShould()
    {
        var validator = Substitute.For<IValidator<CreateFeedRequest>>();
        _storage = Substitute.For<ICreateFeedStorage>();
        _identity = Substitute.For<IIdentity>();
        var identityProvider = Substitute.For<IIdentityProvider>();
        identityProvider.Current.Returns(_identity);

        _sut = new CreateFeedUseCase(_storage, validator, identityProvider);
    }

    [Fact]
    public async Task ReturnResponseWithCreatedFeedId_WhenFeedIsCreated()
    {
        var feedId = Guid.Parse("DF31B440-DC70-46DB-BA04-9633E6E88980");
        var request = new CreateFeedRequest("Test name", "www.test-name.com");
        _identity.Role.Returns("admin");
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
    public async Task ReturnResponseWithStoredFeedId_WhenFeedIsExists()
    {
        var feedId = Guid.Parse("73FC3772-87E5-4205-9E93-D3732BB19D2F");
        var request = new CreateFeedRequest("Feed name", "https://feed-name.com");
        _identity.Role.Returns("admin");
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
    public async Task ThrowForbiddenException_WhenUserIsNotAdmin()
    {
        var request = new CreateFeedRequest("Feed name", "https://feed-name.com");
        _identity.Role.Returns("base_user");

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<ForbiddenException>();
        await _storage
            .Received(0)
            .CreateFeed(Arg.Any<string>(),
                Arg.Any<string>(),
                CancellationToken.None);
    }
}