using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.Identity;
using RssAggregator.Application.UseCases.Feeds.GetUserFeeds;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Feeds.GetUserFeeds;

public class GetUserFeedsUseCaseShould
{
    private readonly GetUserFeedsUseCase _sut;
    private readonly IGetUserFeedsStorage _storage;
    private readonly IIdentity _identity;

    private class TestSpecification : Specification<Feed>;

    public GetUserFeedsUseCaseShould()
    {
        var validator = Substitute.For<IValidator<GetUserFeedsRequest>>();
        _storage = Substitute.For<IGetUserFeedsStorage>();
        _identity = Substitute.For<IIdentity>();
        var identityProvider = Substitute.For<IIdentityProvider>();
        identityProvider.Current.Returns(_identity);

        _sut = new GetUserFeedsUseCase(_storage, validator, identityProvider);
    }

    [Fact]
    public async Task ReturnFeeds_WhenAllGood()
    {
        var feeds = GenerateFeeds(10);
        var request = new GetUserFeedsRequest(new TestSpecification());
        _identity.UserId.Returns(Guid.Parse("F834F023-1CB7-4FD1-BCA2-7DFA64DEEFCF"));
        _storage
            .GetUserFeeds(Arg.Any<Guid>(), Arg.Any<Specification<Feed>>(), CancellationToken.None)
            .Returns(feeds);
        var expected = new GetUserFeedsResponse(feeds);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThrowNotAuthenticatedException_WhenUserIsNotAuthenticated()
    {
        var request = new GetUserFeedsRequest(new TestSpecification());
        _identity.UserId.Returns(Guid.Empty);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NotAuthenticatedException>();
        await _storage
            .Received(0)
            .GetUserFeeds(Arg.Any<Guid>(), Arg.Any<Specification<Feed>>(), CancellationToken.None);
    }

    private static Feed[] GenerateFeeds(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Feed
            {
                Id = Guid.NewGuid(),
                Name = $"Feed {i}",
                Url = $"https://feeds.com/import/{i}/rss.xml"
            })
            .ToArray();
    }
}