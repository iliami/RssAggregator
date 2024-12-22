using FluentAssertions;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.Tests.Params;

public class SortingParamsValidatorShould
{
    private readonly SortingParamsValidator _sut = new();

    [Theory]
    [MemberData(nameof(GetValidSortingParams))]
    public void ReturnSuccess_WhenSortingParamsAreValid(SortingParams sortingParams)
    {
        var actual = _sut.Validate(sortingParams);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ReturnFailure_WhenSortingParamsAreInvalid()
    {
        var sortingParams = new SortingParams(null);

        var actual = _sut.Validate(sortingParams);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetValidSortingParams()
    {
        yield return [new SortingParams()];
        yield return [new SortingParams("some field name")];
        yield return [new SortingParams("some other field name", SortDirection.Asc)];
    }
    
    
}