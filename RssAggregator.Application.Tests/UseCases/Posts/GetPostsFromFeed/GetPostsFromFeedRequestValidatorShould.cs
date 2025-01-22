using FluentAssertions;
using RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Posts.GetPostsFromFeed;

public class GetPostsFromFeedRequestValidatorShould
{
    private readonly GetPostsFromFeedRequestValidator _sut = new();

    private class TestSpecification : Specification<Post>;

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var feedId = Guid.Parse("C7C2237B-B47C-4AEB-ACC0-0D165D20FE30");
        var request = new GetPostsFromFeedRequest(feedId, new TestSpecification());

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequest))]
    public void ReturnFailure_WhenRequestIsInvalid(GetPostsFromFeedRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequest()
    {
        var feedId = Guid.Parse("C7C2237B-B47C-4AEB-ACC0-0D165D20FE30");
        var request = new GetPostsFromFeedRequest(feedId, new TestSpecification());

        yield return [request with { FeedId = Guid.Empty }];
        yield return [request with { Specification = null! }];
    }
}