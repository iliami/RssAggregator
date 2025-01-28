using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.Identity;
using RssAggregator.Application.UseCases.Posts.GetUserPosts;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Posts.GetUserPosts;

public class GetUserPostsUseCaseShould
{
    private readonly GetUserPostsUseCase _sut;
    private readonly IGetUserPostsStorage _storage;
    private readonly IIdentity _identity;

    private class TestSpecification : Specification<Post>;

    public GetUserPostsUseCaseShould()
    {
        var validator = Substitute.For<IValidator<GetUserPostsRequest>>();
        _storage = Substitute.For<IGetUserPostsStorage>();
        _identity = Substitute.For<IIdentity>();
        var identityProvider = Substitute.For<IIdentityProvider>();
        identityProvider.Current.Returns(_identity);

        _sut = new GetUserPostsUseCase(_storage, validator, identityProvider);
    }

    [Fact]
    public async Task ReturnResponseWithPosts_WhenAllGood()
    {
        var request = new GetUserPostsRequest(new TestSpecification());
        _identity.UserId.Returns(Guid.Parse("F9F40938-1954-4938-B8D2-534C790BE9A8"));
        _storage
            .GetUserPosts(
                Arg.Any<Guid>(), 
                Arg.Any<Specification<Post>>(), 
                CancellationToken.None)
            .Returns([]);
        var expected = new GetUserPostsResponse([]);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThrowNotAuthenticatedException_WhenUserIsNotAuthenticated()
    {
        var request = new GetUserPostsRequest(new TestSpecification());
        _identity.UserId
            .Returns(Guid
                .Empty); // TODO: should be smth like _identityProvider.Current.IsAuthenticated().Returns(false);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NotAuthenticatedException>();
        await _storage.Received(0)
            .GetUserPosts(Arg.Any<Guid>(), Arg.Any<Specification<Post>>(), CancellationToken.None);
    }
}