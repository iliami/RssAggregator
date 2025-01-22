using FluentAssertions;
using RssAggregator.Application.UseCases.Posts.GetPost;

namespace RssAggregator.Application.Tests.UseCases.Posts.GetPost;

public class GetPostRequestValidatorShould
{
    private readonly GetPostRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var postId = Guid.Parse("8714E4E0-9E0B-4B83-B2FC-17A12A114962");
        var request = new GetPostRequest(postId);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ReturnFailure_WhenRequestIsInvalid()
    {
        var postId = Guid.Empty;
        var request = new GetPostRequest(postId);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
}