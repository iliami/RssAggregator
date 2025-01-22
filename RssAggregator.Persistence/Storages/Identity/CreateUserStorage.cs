using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RssAggregator.Application.UseCases.Identity.CreateUser;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages.Identity
{
    public class CreateUserStorage(AppDbContext dbContext) : ICreateUserStorage
    {
        public async Task CreateUser(Guid id, CancellationToken ct = default)
        {
            var isUserStored = dbContext.Users.Any(u => u.Id == id);
            if (isUserStored) return;

            var user = new User { Id = id };
            await dbContext.Users.AddAsync(user, ct);
            await dbContext.SaveChangesAsync(ct);
        }
    }
}
