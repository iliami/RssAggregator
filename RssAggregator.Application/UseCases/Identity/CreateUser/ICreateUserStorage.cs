namespace RssAggregator.Application.UseCases.Identity.CreateUser;

public interface ICreateUserStorage
{
    Task CreateUser(Guid Id, CancellationToken ct = default);
}
