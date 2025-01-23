using RssAggregator.Application.UseCases.Identity.CreateUser;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages.Identity
{
    public class CreateUserStorage(AppDbContext dbContext) : ICreateUserStorage
    {
        public async Task<bool> CreateUser(Guid id, CancellationToken ct = default)
        {
            var isUserStored = dbContext.Users.Any(u => u.Id == id);
            if (isUserStored) return true;

            var user = new User { Id = id };
            await dbContext.Users.AddAsync(user, ct);
            await dbContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
