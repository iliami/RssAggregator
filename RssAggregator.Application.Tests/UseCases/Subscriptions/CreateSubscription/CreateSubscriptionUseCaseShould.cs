using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.Auth;
using RssAggregator.Application.UseCases.Subscriptions.CreateSubscriptionUseCase;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Subscriptions.CreateSubscription;

public class CreateSubscriptionUseCaseShould
{
    private readonly ICreateSubscriptionStorage _storage;
    private readonly IIdentity _identity;
    private readonly CreateSubscriptionUseCase _sut;

    public CreateSubscriptionUseCaseShould()
    {
        var validator = Substitute.For<IValidator<CreateSubscriptionRequest>>();
        _storage = Substitute.For<ICreateSubscriptionStorage>();
        _identity = Substitute.For<IIdentity>();
        var identityProvider = Substitute.For<IIdentityProvider>();
        identityProvider.Current.Returns(_identity);

        _sut = new CreateSubscriptionUseCase(_storage, validator, identityProvider);
    }

    [Fact]
    public async Task ReturnResponse_WhenSubscriptionIsCreated()
    {
        var feedId = Guid.Parse("1FDC58BA-717B-44C2-BA93-4C13A4B76E7A");
        var request = new CreateSubscriptionRequest(feedId);
        _identity.UserId.Returns(Guid.Parse("664FD68B-2333-41E7-937D-329D2F85B165"));
        _storage
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(true);
        _storage
            .CreateSubscription(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None)
            .Returns(true);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().NotBeNull().And.BeOfType<CreateSubscriptionResponse>();
    }

    [Fact]
    public async Task ThrowNoAccessException_WhenUserIsNotAuthenticated()
    {
        var feedId = Guid.Parse("1FDC58BA-717B-44C2-BA93-4C13A4B76E7A");
        var request = new CreateSubscriptionRequest(feedId);
        _identity.UserId.Returns(Guid.Empty);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NoAccessException>();
        await _storage.Received(0)
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None);
        await _storage.Received(0)
            .CreateSubscription(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None);
    }

    [Fact]
    public async Task ThrowNotFoundExceptionOfFeed_WhenFeedNotExists()
    {
        var feedId = Guid.Parse("1FDC58BA-717B-44C2-BA93-4C13A4B76E7A");
        var request = new CreateSubscriptionRequest(feedId);
        _identity.UserId.Returns(Guid.Parse("664FD68B-2333-41E7-937D-329D2F85B165"));
        _storage
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(false);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NotFoundException<Feed>>();
        await _storage.Received(0)
            .CreateSubscription(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None);
    }

    [Fact]
    public async Task ThrowNotFoundExceptionOfAppUser_WhenAppUserNotExists()
    {
        var feedId = Guid.Parse("1FDC58BA-717B-44C2-BA93-4C13A4B76E7A");
        var request = new CreateSubscriptionRequest(feedId);
        _identity.UserId.Returns(Guid.Parse("664FD68B-2333-41E7-937D-329D2F85B165"));
        _storage
            .IsFeedExist(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(true);
        _storage
            .CreateSubscription(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None)
            .Returns(false);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NotFoundException<User>>();
    }
}