using FluentAssertions;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Posts.GetPost;

public class GetPostRequestValidatorShould
{
    private readonly GetPostRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var postId = Guid.Parse("8714E4E0-9E0B-4B83-B2FC-17A12A114962");
        var request = new GetPostRequest(postId, new TestSpecification());

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(GetPostRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        var postId = Guid.Parse("B8715F05-5367-43D3-A424-564320E97B51");
        var request = new GetPostRequest(postId, new TestSpecification());

        yield return [ request with { PostId = Guid.Empty } ];
        yield return [ request with { Specification = null!} ];
    }
    private class TestSpecification : Specification<Post>;
}