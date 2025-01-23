using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.UseCases.Identity.CreateUser;

namespace RssAggregator.Application.Tests.UseCases.Identity.CreateUser;

public class CreateUserUseCaseShould
{
    private readonly CreateUserUseCase _sut;
    private readonly ICreateUserStorage _storage;

    public CreateUserUseCaseShould()
    {
        var validator = Substitute.For<IValidator<CreateUserRequest>>();
        _storage = Substitute.For<ICreateUserStorage>();

        _sut = new CreateUserUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnResponse_WhenUserIsCreated()
    {
        var userId = Guid.Parse("E75FC61A-C8DA-4BDF-B541-4BEB08DCBBD6");
        var request = new CreateUserRequest(userId);
        _storage.CreateUser(Arg.Any<Guid>(), CancellationToken.None).Returns(true);

        var actual = await _sut.Handle(request);

        actual.Should().NotBeNull().And.BeOfType<CreateUserResponse>();
    }
}