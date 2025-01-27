using FluentAssertions;
using Iliami.Identity.Domain.UseCases.Tokens.RevokeTokens;

namespace Iliami.Identity.Domain.Tests.UseCases.Tokens.RevokeTokens;

public class RevokeTokensRequestValidatorShould
{
    private readonly RevokeTokensRequestValidator _sut = new();
    
    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var userId = Guid.Parse("1EEC8CDE-15D4-4051-AB2E-A8A8B693E858");
        var request = new RevokeTokensRequest(userId);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ReturnFailure_WhenRequestIsInvalid()
    {
        var userId = Guid.Empty;
        var request = new RevokeTokensRequest(userId);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
}