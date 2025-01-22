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
        public async Task CreateUser(Guid Id, CancellationToken ct = default)
        {
            var user = new User { Id = Id };
            await dbContext.Users.AddAsync(user, ct);
            await dbContext.SaveChangesAsync(ct);
        }
    }
}
