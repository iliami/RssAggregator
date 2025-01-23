using FluentAssertions;
using RssAggregator.Application.UseCases.Feeds.GetUserFeeds;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Feeds.GetUserFeeds;

public class GetUserFeedsRequestValidatorShould
{
    private readonly GetUserFeedsRequestValidator _sut = new();

    private class TestSpecification : Specification<Feed>;

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var request = new GetUserFeedsRequest(new TestSpecification());

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ReturnFailure_WhenRequestIsInvalid()
    {
        var request = new GetUserFeedsRequest(null!);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
}