using FluentAssertions;
using RssAggregator.Application.UseCases.Identity.CreateUser;

namespace RssAggregator.Application.Tests.UseCases.Identity.CreateUser;

public class CreateUserRequestValidatorShould
{
    private readonly CreateUserRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var userId = Guid.Parse("E75FC61A-C8DA-4BDF-B541-4BEB08DCBBD6");
        var request = new CreateUserRequest(userId);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ReturnFailure_WhenRequestIsInvalid()
    {
        var userId = Guid.Empty;
        var request = new CreateUserRequest(userId);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
}