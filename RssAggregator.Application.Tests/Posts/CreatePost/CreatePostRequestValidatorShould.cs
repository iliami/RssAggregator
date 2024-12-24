using FluentAssertions;
using RssAggregator.Application.UseCases.Posts.CreatePost;

namespace RssAggregator.Application.Tests.Posts.CreatePost;

public class CreatePostRequestValidatorShould
{
    private readonly CreatePostRequestValidator _sut = new();
    private static readonly CreatePostRequest ValidRequest = new(
        "Post title",
        "Post description",
        ["Category"],
        DateTime.UtcNow.AddHours(-1),
        "http://www.feed.post.com",
        Guid.Parse("2F3B02A4-7F20-4BD2-92DF-7EF384BAF418"));

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var actual = _sut.Validate(ValidRequest);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(CreatePostRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return [ValidRequest with { Title = null! }];
        yield return [ValidRequest with { Title = new string('a', 1025) }];
        yield return [ValidRequest with { Description = null! }];
        yield return [ValidRequest with { Description = new string('a', 32769) }];
        yield return [ValidRequest with { Categories = null! }];
        yield return [ValidRequest with { Categories = [new string('a', 65)] }];
        yield return [ValidRequest with { Url = "" }];
        yield return [ValidRequest with { Url = string.Concat(Enumerable.Repeat("example.", 30)) + "8times30.equal240" }];
        yield return [ValidRequest with { Url = "not a url" }];
        yield return [ValidRequest with { Url = "https:/invalid.url.com" }];
        yield return [ValidRequest with { Url = "https:://invalid.url.com" }];
        yield return [ValidRequest with { Url = "htps://invalid.url.com" }];
        yield return [ValidRequest with { Url = "htpps:invalid.url.com" }];
        yield return [ValidRequest with { PublishDate = DateTime.MinValue }];
        yield return [ValidRequest with { PublishDate = DateTime.UtcNow.AddMinutes(1) }];
        yield return [ValidRequest with { FeedId = Guid.Empty }];
    }
}