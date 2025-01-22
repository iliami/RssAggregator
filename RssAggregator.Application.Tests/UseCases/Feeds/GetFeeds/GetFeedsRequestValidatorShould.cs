using FluentAssertions;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Feeds.GetFeeds;

public class GetFeedsRequestValidatorShould
{
    private class TestSpecification : Specification<Feed>;

    private readonly GetFeedsRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var specification = new TestSpecification();
        var request = new GetFeedsRequest(specification);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(GetFeedsRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return [new GetFeedsRequest(null!)];
    }
}