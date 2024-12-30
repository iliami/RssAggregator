using FluentAssertions;
using RssAggregator.Application.UseCases.Feeds.UpdateFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Feeds.UpdateFeed;

public class UpdateFeedRequestValidatorShould
{
    private readonly UpdateFeedRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenFeedIsNotNull()
    {
        var feed = new Feed
        {
            Id = Guid.Parse("49A6FC5A-6F3D-4BCF-93A9-B2ED8204CEEA"),
            Name = "Feed Name",
            Url = "www.example.com",
            LastFetchedAt = DateTimeOffset.Now.AddDays(-1)
        };
        var request = new UpdateFeedRequest(feed);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ReturnFailure_WhenFeedIsNull()
    {
        var request = new UpdateFeedRequest(null!);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
}