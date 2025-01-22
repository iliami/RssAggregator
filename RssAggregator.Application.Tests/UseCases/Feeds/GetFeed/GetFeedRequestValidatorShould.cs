using FluentAssertions;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Feeds.GetFeed;

public class GetFeedRequestValidatorShould
{
    private class TestSpecification : Specification<Feed>;

    private readonly GetFeedRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var feedId = Guid.Parse("CAE03431-B05B-4B8C-A15C-4B66CE62ABF6");
        var specification = new TestSpecification();
        var request = new GetFeedRequest(feedId, specification);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(GetFeedRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        var feedId = Guid.Parse("CAE03431-B05B-4B8C-A15C-4B66CE62ABF6");
        var specification = new TestSpecification();
        var request = new GetFeedRequest(feedId, specification);

        yield return [request with { FeedId = Guid.Empty }];
        yield return [request with { Specification = null! }];
    }
}