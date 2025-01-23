using FluentAssertions;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.Storages.Identity;

namespace RssAggregator.Persistence.Tests.Storages.Identity;

public class CreateUserStorageShould
{
    [Fact]
    public async Task ReturnTrue_WhenUserIsCreated()
    {
        var dbContext = new TestDbContext();
        var sut = new CreateUserStorage(dbContext);
        var userId = Guid.Parse("DFB8F872-62EF-428C-A426-E3823A45880C");

        var actual = await sut.CreateUser(userId);

        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnTrue_WhenUserIsStored()
    {
        var dbContext = new TestDbContext();
        var sut = new CreateUserStorage(dbContext);
        var userId = Guid.Parse("E28C18A9-094C-4FCF-9059-10FB21647072");
        var user = new User { Id = userId };
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var actual = await sut.CreateUser(userId);

        actual.Should().BeTrue();
    }
}