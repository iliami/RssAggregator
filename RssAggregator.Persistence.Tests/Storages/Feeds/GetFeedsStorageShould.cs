using FluentAssertions;
using RssAggregator.Application;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Feeds;

namespace RssAggregator.Persistence.Tests.Storages.Feeds;

public class GetFeedsStorageShould
{
    [Fact]
    public async Task ReturnFeeds_WhenAllGood()
    {
        var dbContext = new TestDbContext();
        var sut = new GetFeedsStorage(dbContext);
        var feeds = GenerateFeeds(20);
        await dbContext.Feeds.AddRangeAsync(feeds);
        await dbContext.SaveChangesAsync();

        var actual = await sut.GetFeeds(new TestSpecification());

        actual.Should().BeEquivalentTo(feeds);
    }

    private class TestSpecification : Specification<Feed>;

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