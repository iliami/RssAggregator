namespace RssAggregator.Application.UseCases.Identity.CreateUser;

public interface ICreateUserStorage
{
    Task CreateUser(Guid id, CancellationToken ct = default);
}
