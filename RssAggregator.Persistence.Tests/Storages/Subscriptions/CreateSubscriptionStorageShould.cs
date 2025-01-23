using FluentAssertions;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Subscriptions;

namespace RssAggregator.Persistence.Tests.Storages.Subscriptions;

public class CreateSubscriptionStorageShould
{
     [Fact]
     public async Task ReturnTrue_WhenFeedIsExists()
     {
          var dbContext = new TestDbContext();
          var sut = new CreateSubscriptionStorage(dbContext);
          var feedId = Guid.Parse("5B9AD7AF-02A8-4CA1-A024-BFB2982BB56D");
          var feed = new Feed
          {
               Id = feedId,
               Name = "Feed name",
               Description = "Feed description",
               Url = "https://www.example.com"
          };
          await dbContext.Feeds.AddAsync(feed);
          await dbContext.SaveChangesAsync();

          var actual = await sut.IsFeedExist(feedId);

          actual.Should().BeTrue();
     }

     [Fact]
     public async Task ReturnFalse_WhenFeedIsNotExists()
     {
          var dbContext = new TestDbContext();
          var sut = new CreateSubscriptionStorage(dbContext);
          var feedId = Guid.Parse("591FD634-6031-4FC9-A832-174E80ABDC06");

          var actual = await sut.IsFeedExist(feedId);

          actual.Should().BeFalse();
     }

     [Fact]
     public async Task ReturnTrue_WhenSubscriptionIsCreated()
     {
          var dbContext = new TestDbContext();
          var sut = new CreateSubscriptionStorage(dbContext);
          var userId = Guid.Parse("85523F1A-0C87-4EDF-838A-659F15BE11C9");
          var user = new User { Id = userId };
          var feedId = Guid.Parse("C382AB96-3E48-4CBD-855E-A4156F7E83AB");
          var feed = new Feed
          {
               Id = feedId,
               Name = "Feed name",
               Description = "Feed description",
               Url = "https://www.example.com",
               LastFetchedAt = DateTimeOffset.UtcNow
          };
          await dbContext.Users.AddAsync(user);
          await dbContext.Feeds.AddAsync(feed);
          await dbContext.SaveChangesAsync();

          var actual = await sut.CreateSubscription(userId, feedId, CancellationToken.None);

          actual.Should().BeTrue();
     }

     [Fact]
     public async Task ReturnTrue_WhenSubscriptionIsExists()
     {
          var dbContext = new TestDbContext();
          var sut = new CreateSubscriptionStorage(dbContext);
          var userId = Guid.Parse("E56427E9-F4ED-480C-8617-2D56F4F3E063");
          var user = new User { Id = userId };
          var feedId = Guid.Parse("E7D8B1E2-D29D-4DBD-94FA-43748F97038A");
          var feed = new Feed
          {
               Id = feedId,
               Name = "Feed name",
               Description = "Feed description",
               Url = "https://www.example.com",
               LastFetchedAt = DateTimeOffset.UtcNow,
               Subscribers = [user]
          };
          await dbContext.Users.AddAsync(user);
          await dbContext.Feeds.AddAsync(feed);
          await dbContext.SaveChangesAsync();

          var actual = await sut.CreateSubscription(userId, feedId, CancellationToken.None);

          actual.Should().BeTrue();
     }

     [Fact]
     public async Task ReturnFalse_WhenSubscriptionIsNotCreated_BecauseUserIsNotExists()
     {
          var dbContext = new TestDbContext();
          var sut = new CreateSubscriptionStorage(dbContext);
          var userId = Guid.Parse("BA641C05-919B-4CB1-A261-6C9391497410");
          var feedId = Guid.Parse("D43CE1C2-3458-4B91-A03D-034D22C7BFCA");
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

          var actual = await sut.CreateSubscription(userId, feedId, CancellationToken.None);

          actual.Should().BeFalse();
     }

     [Fact]
     public async Task ReturnFalse_WhenSubscriptionIsNotCreated_BecauseFeedIsNotExists()
     {
          var dbContext = new TestDbContext();
          var sut = new CreateSubscriptionStorage(dbContext);
          var userId = Guid.Parse("FABD0F49-76BC-42C8-AF0A-ED1BD300CEFD");
          var user = new User { Id = userId };
          var feedId = Guid.Parse("AB0824DC-706B-44AA-8847-17FEB6603FF4");
          await dbContext.Users.AddAsync(user);
          await dbContext.SaveChangesAsync();

          var actual = await sut.CreateSubscription(userId, feedId, CancellationToken.None);

          actual.Should().BeFalse();
     }
}