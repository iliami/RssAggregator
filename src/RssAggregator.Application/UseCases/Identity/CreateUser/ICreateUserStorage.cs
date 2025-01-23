namespace RssAggregator.Application.UseCases.Identity.CreateUser;

public interface ICreateUserStorage
{
    Task<bool> CreateUser(Guid id, CancellationToken ct = default);
}
