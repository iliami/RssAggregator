using FluentAssertions;
using Iliami.Identity.Domain.Constants;
using Iliami.Identity.Domain.UseCases.Tokens.RefreshTokens;

namespace Iliami.Identity.Domain.Tests.UseCases.Tokens.RefreshTokens;

public class RefreshTokensRequestValidatorShould
{
    private readonly RefreshTokensRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var request = new RefreshTokensRequest(Guid.NewGuid(), "AA1BC62F8B6548B1BB4FC666D6D2235B");

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(RefreshTokensRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> GetInvalidRequests()
    {
        var request = new RefreshTokensRequest(Guid.NewGuid(), "AA1BC62F8B6548B1BB4FC666D6D2235B");

        yield return [ request with { UserId = Guid.Empty } ];

        yield return [ request with { RefreshToken = null!} ];
        yield return [ request with { RefreshToken = string.Empty} ];
        yield return [ request with { RefreshToken = new string('a', 31) } ];
        yield return [ request with { RefreshToken = new string('a', 33) } ];
    }
}