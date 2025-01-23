using FluentAssertions;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Feeds;

namespace RssAggregator.Persistence.Tests.Storages.Feeds;

public class UpdateFeedStorageShould
{
    [Fact]
    public async Task ReturnTrueAndUpdatedFeedId_WhenFeedIsUpdated()
    {
        var dbContext = new TestDbContext();
        var sut = new UpdateFeedStorage(dbContext);
        var feedId = Guid.Parse("9EE68FB0-A5C0-4B0F-896B-70F55BDD2D9C");
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
        feed.Name = "Test feed name";
        feed.Description = "Test feed description";

        var actual = await sut.TryUpdateFeed(feed);

        actual.Should().BeEquivalentTo((true, feedId));
    }
    
    [Fact]
    public async Task ReturnFalseAndFeedId_WhenFeedIsNotExists()
    {
        var dbContext = new TestDbContext();
        var sut = new UpdateFeedStorage(dbContext);
        var feedId = Guid.Parse("2FB5F4DC-56B2-459A-BF80-46603412D4D9");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Test feed",
            Description = "Test feed",
            Url = "https://www.example.com",
            LastFetchedAt = DateTimeOffset.UtcNow
        };
        feed.Name = "Test feed name";
        feed.Description = "Test feed description";

        var actual = await sut.TryUpdateFeed(feed);

        actual.Should().BeEquivalentTo((false, feedId));
    }
}