using FluentAssertions;
using RssAggregator.Application.UseCases.Posts.GetUserPosts;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Posts.GetUserPosts;

public class GetUserPostsRequestValidatorShould
{
    private readonly GetUserPostsRequestValidator _sut = new();

    private class TestSpecification : Specification<Post>;

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var request = new GetUserPostsRequest(new TestSpecification());

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ReturnFailure_WhenRequestIsInvalid()
    {
        var request = new GetUserPostsRequest(null!);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
}