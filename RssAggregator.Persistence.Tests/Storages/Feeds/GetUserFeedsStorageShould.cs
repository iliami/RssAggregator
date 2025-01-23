using FluentAssertions;
using RssAggregator.Application;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Feeds;

namespace RssAggregator.Persistence.Tests.Storages.Feeds;

public class GetUserFeedsStorageShould
{
    [Fact]
    public async Task ReturnFeeds_ThatUserIsSubscribedTo()
    {
        var dbContext = new TestDbContext();
        var sut = new GetUserFeedsStorage(dbContext);
        var userId = Guid.Parse("A5B81877-94E0-44B4-9E25-29038486F324");
        var user = new User { Id = userId };
        var userFeeds = GenerateFeedsForUser(20, user);
        var notUserFeeds = GenerateFeeds(10); 
        var feeds = userFeeds.Concat(notUserFeeds).ToArray();
        await dbContext.Feeds.AddRangeAsync(feeds);
        await dbContext.SaveChangesAsync();

        var actual = await sut.GetUserFeeds(userId, new TestSpecification());

        actual.Should().BeEquivalentTo(userFeeds);
    }

    private class TestSpecification : Specification<Feed>;

    private static Feed[] GenerateFeedsForUser(int count, User user)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Feed
            {
                Id = Guid.NewGuid(),
                Name = $"Feed {i}",
                Url = $"https://example.com/{i}/import.xml",
                Subscribers = [user]
            })
            .ToArray();
    }

    private static Feed[] GenerateFeeds(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Feed
            {
                Id = Guid.NewGuid(),
                Name = $"Feed {i}",
                Url = $"https://feeds.com/import/{i}/rss.xml"
            })
            .ToArray();
    }
}