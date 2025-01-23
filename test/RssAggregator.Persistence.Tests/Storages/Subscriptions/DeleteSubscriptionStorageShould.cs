using FluentAssertions;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Subscriptions;

namespace RssAggregator.Persistence.Tests.Storages.Subscriptions;

public class DeleteSubscriptionStorageShould
{
     [Fact]
     public async Task ReturnTrue_WhenFeedIsExists()
     {
          var dbContext = new TestDbContext();
          var sut = new DeleteSubscriptionStorage(dbContext);
          var feedId = Guid.Parse("5D9C929E-078A-440B-86AC-351DD2FA8771");
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
          var sut = new DeleteSubscriptionStorage(dbContext);
          var feedId = Guid.Parse("2DA397AC-B0DB-4B5F-90D6-27262BD936CB");

          var actual = await sut.IsFeedExist(feedId);

          actual.Should().BeFalse();
     }

     [Fact]
     public async Task ReturnTrue_WhenSubscriptionIsDeleted()
     {
          var dbContext = new TestDbContext();
          var sut = new DeleteSubscriptionStorage(dbContext);
          var userId = Guid.Parse("ECD5BFED-7336-4B33-9945-1C019F0D3301");
          var user = new User { Id = userId };
          var feedId = Guid.Parse("FDEBA580-13AC-4C85-8712-8EA70906D62E");
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

          var actual = await sut.DeleteSubscription(userId, feedId, CancellationToken.None);

          actual.Should().BeTrue();
     }

     [Fact]
     public async Task ReturnTrue_WhenSubscriptionIsNotExists()
     {
          var dbContext = new TestDbContext();
          var sut = new DeleteSubscriptionStorage(dbContext);
          var userId = Guid.Parse("B68316FE-389A-4A45-9DE3-8288CDD02155");
          var user = new User { Id = userId };
          var feedId = Guid.Parse("CA412B5F-5D91-44B7-A446-D5F21BE9D2B5");
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

          var actual = await sut.DeleteSubscription(userId, feedId, CancellationToken.None);

          actual.Should().BeTrue();
     }

     [Fact]
     public async Task ReturnFalse_WhenSubscriptionIsNotDeleted_BecauseUserIsNotExists()
     {
          var dbContext = new TestDbContext();
          var sut = new DeleteSubscriptionStorage(dbContext);
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

          var actual = await sut.DeleteSubscription(userId, feedId, CancellationToken.None);

          actual.Should().BeFalse();
     }

     [Fact]
     public async Task ReturnFalse_WhenSubscriptionIsNotDeleted_BecauseFeedIsNotExists()
     {
          var dbContext = new TestDbContext();
          var sut = new DeleteSubscriptionStorage(dbContext);
          var userId = Guid.Parse("FABD0F49-76BC-42C8-AF0A-ED1BD300CEFD");
          var user = new User { Id = userId };
          var feedId = Guid.Parse("AB0824DC-706B-44AA-8847-17FEB6603FF4");
          await dbContext.Users.AddAsync(user);
          await dbContext.SaveChangesAsync();

          var actual = await sut.DeleteSubscription(userId, feedId, CancellationToken.None);

          actual.Should().BeFalse();
     }
}