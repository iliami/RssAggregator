using FluentAssertions;
using RssAggregator.Application.Params;

namespace RssAggregator.Application.Tests.Params;

public class PaginationParamsValidatorShould
{
    private readonly PaginationParamsValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenPaginationParamsAreValid()
    {
        var paginationParams = new PaginationParams(1, 10);

        var actual = _sut.Validate(paginationParams);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidPaginationParams))]
    public void ReturnFailure_WhenPaginationParamsAreInvalid(PaginationParams paginationParams)
    {
        var actual = _sut.Validate(paginationParams);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidPaginationParams()
    {
        var paginationParams = new PaginationParams(1, 10);

        yield return [paginationParams with { Page = 0 }];
        yield return [paginationParams with { Page = -1 }];
        yield return [paginationParams with { PageSize = -1 }];
    }
}