using FluentAssertions;
using Iliami.Identity.Domain.UseCases.Tokens.GenerateTokens;

namespace Iliami.Identity.Domain.Tests.UseCases.Tokens.GenerateTokens;

public class GenerateTokensRequestValidatorShould
{
    private readonly GenerateTokensRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var request = new GenerateTokensRequest(Guid.NewGuid(), RoleConstants.Base);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(GenerateTokensRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> GetInvalidRequests()
    {
        var request = new GenerateTokensRequest(Guid.NewGuid(), RoleConstants.Base);

        yield return [ request with { UserId = Guid.Empty } ];

        yield return [ request with { Role = null!} ];
        yield return [ request with { Role = string.Empty} ];
    }
}