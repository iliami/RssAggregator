using FluentAssertions;
using RssAggregator.Application;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Feeds;

namespace RssAggregator.Persistence.Tests.Storages.Feeds;

public class GetFeedStorageShould
{
    [Fact]
    public async Task ReturnTrueAndFeed_WhenFeedIsExists()
    {
        var dbContext = new TestDbContext();
        var sut = new GetFeedStorage(dbContext);
        var feedId = Guid.Parse("58A6210C-2F0C-4820-9D79-6D2309EFCAF0");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Test feed",
            Description = "Test feed",
            Url = "https://www.example.com",
            LastFetchedAt = DateTimeOffset.UtcNow
        };
        await dbContext.Feeds.AddAsync(feed);
        await dbContext.SaveChangesAsync();

        var actual = await sut.TryGetFeed(feedId, new TestSpecification(), CancellationToken.None);

        actual.Should().BeEquivalentTo((true, feed));
    }
    
    [Fact]
    public async Task ReturnFalseAndNull_WhenFeedIsNotExists()
    {
        var dbContext = new TestDbContext();
        var sut = new GetFeedStorage(dbContext);
        var feedId = Guid.Parse("CAD808F1-47CC-4EC6-B940-302343FC9176");

        var actual = await sut.TryGetFeed(feedId, new TestSpecification(), CancellationToken.None);

        actual.Should().BeEquivalentTo<(bool, Feed)>((false, null!));
    }
    private class TestSpecification : Specification<Feed>;
}