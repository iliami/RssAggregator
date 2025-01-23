using FluentAssertions;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Feeds;

namespace RssAggregator.Persistence.Tests.Storages.Feeds;

public class CreateFeedStorageShould
{
    [Fact]
    public async Task ReturnCreatedFeedId_WhenUserIsCreated()
    {
        var dbContext = new TestDbContext();
        var sut = new CreateFeedStorage(dbContext);        

        var actual = await sut.CreateFeed("Feed name", "www.example.com");

        actual.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ReturnTrue_WhenUserIsStored()
    {
        var dbContext = new TestDbContext();
        var sut = new CreateFeedStorage(dbContext);
        var feedId = Guid.Parse("6CD9D1F5-5205-45C8-9B31-C8017D0A735E");
        var feed = new Feed
        {
            Id = feedId,
            Name = "Feed name",
            Description = "Feed description",
            Url = "https://www.example.com",
            LastFetchedAt = DateTimeOffset.UtcNow
        };
        await dbContext.Feeds.AddAsync(feed);
        await dbContext.SaveChangesAsync();

        var actual = await sut.CreateFeed(feed.Name, feed.Url);

        actual.Should().Be(feedId);
    }
}