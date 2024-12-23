using FluentAssertions;
using RssAggregator.Application.UseCases.Feeds.CreateFeed;

namespace RssAggregator.Application.Tests.Feeds.CreateFeed;

public class CreateFeedRequestValidatorShould
{
    private readonly CreateFeedRequestValidator _sut = new();
    private static readonly CreateFeedRequest ValidRequest = new("SOME FEED NAME", "https://example.com/");
    
    [Theory]
    [MemberData(nameof(GetValidRequests))]
    public void ReturnSuccess_WhenRequestIsValid(CreateFeedRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(CreateFeedRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return [ValidRequest with { Name = null! }];
        yield return [ValidRequest with { Name = new string('F', 129) }];
        yield return [ValidRequest with { Url = null! }];
        yield return [ValidRequest with { Url = "" }];
        yield return [ValidRequest with { Url = string.Concat(Enumerable.Repeat("example.", 30)) + "8times30.equal240" }];
        yield return [ValidRequest with { Url = "not a url" }];
        yield return [ValidRequest with { Url = "https:/not-a-url.com" }];
        yield return [ValidRequest with { Url = "https:://not-a-url.com" }];
        yield return [ValidRequest with { Url = "htps://not-a-url.com" }];
        yield return [ValidRequest with { Url = "htpps:not-a-url.com" }];
    }
    
    public static IEnumerable<object[]> GetValidRequests()
    {
        yield return [ValidRequest with { Name = new string('F', 0) }];
        yield return [ValidRequest with { Name = new string('F', 128) }];
        yield return [ValidRequest with { Url = new string('u', 252) + ".com" }];
        yield return [ValidRequest with { Url = "example.com/some/url/to/file.xml?somequery1=value1&somequery2=value2" }];
        yield return [ValidRequest with { Url = "www.example.com/some/url/to/file.xml?somequery1=value1&somequery2=value2" }];
        yield return [ValidRequest with { Url = "http://example.com" }];
        yield return [ValidRequest with { Url = "http://example.com/some/url/to/file.xml" }];
        yield return [ValidRequest with { Url = "https://example.com" }];
        yield return [ValidRequest with { Url = "https://example.com/some/url/to/file.xml?somequery1=value1&somequery2=value2" }];
        yield return [ValidRequest with { Url = "http://www.example.com" }];
        yield return [ValidRequest with { Url = "https://www.example.com" }];
        yield return [ValidRequest];
    }
}