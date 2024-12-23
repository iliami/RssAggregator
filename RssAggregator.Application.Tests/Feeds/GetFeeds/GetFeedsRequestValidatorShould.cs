using FluentAssertions;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;

namespace RssAggregator.Application.Tests.Feeds.GetFeeds;

public class GetFeedsRequestValidatorShould
{
    private readonly GetFeedsRequestValidator _sut = new();
    
    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var request = new GetFeedsRequest(
            new PaginationParams(),
            new SortingParams());

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
        var request = new GetFeedsRequest(
            new PaginationParams(),
            new SortingParams());
        
        yield return [request with { PaginationParams = null! }];
        yield return [request with { SortingParams = null! }];
    }
}