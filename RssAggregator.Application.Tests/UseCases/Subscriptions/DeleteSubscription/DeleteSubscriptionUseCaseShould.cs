using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.Auth;
using RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Subscriptions.DeleteSubscription;

public class DeleteSubscriptionUseCaseShould
{
    private readonly IDeleteSubscriptionStorage _storage;
    private readonly IIdentity _identity;
    private readonly DeleteSubscriptionUseCase _sut;

    public DeleteSubscriptionUseCaseShould()
    {
        var validator = Substitute.For<IValidator<DeleteSubscriptionRequest>>();
        _storage = Substitute.For<IDeleteSubscriptionStorage>();
        _identity = Substitute.For<IIdentity>();
        var identityProvider = Substitute.For<IIdentityProvider>();
        identityProvider.Current.Returns(_identity);

        _sut = new DeleteSubscriptionUseCase(_storage, validator, identityProvider);
    }

    [Fact]
    public async Task ReturnResponse_WhenSubscriptionIsDeleted()
    {
        var feedId = Guid.Parse("BC41D1F3-C74B-43DB-9F03-1B07EC247603");
        var request = new DeleteSubscriptionRequest(feedId);
        _identity.UserId.Returns(Guid.Parse("EAFCE479-7461-46BF-BC14-CB7E5AEA1914"));
        _storage
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(true);
        _storage
            .DeleteSubscription(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None)
            .Returns(true);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().NotBeNull().And.BeOfType<DeleteSubscriptionResponse>();
    }

    [Fact]
    public async Task ThrowNoAccessException_WhenUserIsNotAuthenticated()
    {
        var feedId = Guid.Parse("BC41D1F3-C74B-43DB-9F03-1B07EC247603");
        var request = new DeleteSubscriptionRequest(feedId);
        _identity.UserId.Returns(Guid.Empty);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NoAccessException>();
        await _storage.Received(0)
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None);
        await _storage.Received(0)
            .DeleteSubscription(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None);
    }

    [Fact]
    public async Task ThrowNotFoundExceptionOfFeed_WhenFeedNotExists()
    {
        var feedId = Guid.Parse("BC41D1F3-C74B-43DB-9F03-1B07EC247603");
        var request = new DeleteSubscriptionRequest(feedId);
        _identity.UserId.Returns(Guid.Parse("EAFCE479-7461-46BF-BC14-CB7E5AEA1914"));
        _storage
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(false);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NotFoundException<Feed>>();
        await _storage.Received(0)
            .DeleteSubscription(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None);
    }

    [Fact]
    public async Task ThrowNotFoundExceptionOfAppUser_WhenAppUserNotExists()
    {
        var feedId = Guid.Parse("BC41D1F3-C74B-43DB-9F03-1B07EC247603");
        var request = new DeleteSubscriptionRequest(feedId);
        _identity.UserId.Returns(Guid.Parse("EAFCE479-7461-46BF-BC14-CB7E5AEA1914"));
        _storage
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(true);
        _storage
            .DeleteSubscription(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None)
            .Returns(false);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NotFoundException<User>>();
    }
}