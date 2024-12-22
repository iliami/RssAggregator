using FluentAssertions;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Posts.GetPosts;

namespace RssAggregator.Application.Tests.Posts.GetPosts;

public class GetPostsRequestValidatorShould
{
    private readonly GetPostsRequestValidator _sut = new();
    
    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var request = new GetPostsRequest(
            new PaginationParams(),
            new SortingParams(),
            new PostFilterParams([]));

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(GetPostsRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        var request = new GetPostsRequest(
            new PaginationParams(),
            new SortingParams(),
            new PostFilterParams([]));
        
        yield return [request with { PaginationParams = null! }];
        yield return [request with { SortingParams = null! }];
        yield return [request with { FilterParams = null! }];
    }
}