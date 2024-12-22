using FluentAssertions;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.Tests.Params;

public class PostFilterParamsValidatorShould
{
    private readonly PostFilterParamsValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenPaginationParamsAreValid()
    {
        var postFilterParams = new PostFilterParams([]);

        var actual = _sut.Validate(postFilterParams);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ReturnFailure_WhenPaginationParamsAreInvalid()
    {
        var postFilterParams = new PostFilterParams(null!);

        var actual = _sut.Validate(postFilterParams);

        actual.IsValid.Should().BeFalse();
    }
}