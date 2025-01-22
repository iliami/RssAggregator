using FluentAssertions;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Posts.GetPosts;

public class GetPostsRequestValidatorShould
{
    private class TestSpecification : Specification<Post>;

    private readonly GetPostsRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var request = new GetPostsRequest(new TestSpecification());

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
        yield return [new GetPostsRequest(Specification: null!)];
    }
}