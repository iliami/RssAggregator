using FluentAssertions;
using Iliami.Identity.Domain.UseCases.Users.CreateUser;

namespace Iliami.Identity.Domain.Tests.UseCases.Users.CreateUser;

public class CreateUserRequestValidatorShould
{
    private readonly CreateUserRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var request = new CreateUserRequest("admin@microsoft.com", "password");

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(CreateUserRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> GetInvalidRequests()
    {
        var request = new CreateUserRequest("admin@microsoft.com", "password");

        yield return [ request with { Email = null! } ];
        yield return [ request with { Email = string.Empty } ];
        yield return [ request with { Email = "NotEmail" } ];
        yield return [ request with { Email = "The33CharLongEmail@SomeDomain.com" } ];

        yield return [ request with { Password = null!} ];
        yield return [ request with { Password = string.Empty} ];
        yield return [ request with { Password = new string('a', 7)} ];
        yield return [ request with { Password = new string('a', 65)} ];
        yield return [ request with { Password = "PasswordWith\nNewLine"} ];
        yield return [ request with { Password = "PasswordWith Space"} ];
        yield return [ request with { Password = "PasswordWith\tTab"} ];
    }
}