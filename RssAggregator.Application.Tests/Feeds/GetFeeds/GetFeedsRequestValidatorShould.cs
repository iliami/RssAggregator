using FluentAssertions;
using NSubstitute;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.Feeds.GetFeeds;

public class GetFeedsRequestValidatorShould
{
    private class TestSpecification : Specification<Feed> {}
    
    private readonly GetFeedsRequestValidator<Feed> _sut = new();
    
    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var specification = new TestSpecification();
        var request = new GetFeedsRequest<Feed>(specification);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(GetFeedsRequest<Feed> request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return [new GetFeedsRequest<Feed>(null!) ];
    }
}