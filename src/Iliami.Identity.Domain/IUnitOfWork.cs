namespace Iliami.Identity.Domain;

public interface IUnitOfWork
{
    Task<IUnitOfWorkScope> StartScope(CancellationToken ct = default);
}

public interface IUnitOfWorkScope : IAsyncDisposable
{
    TStorage GetStorage<TStorage>() where TStorage : IStorage;
    Task Commit(CancellationToken ct = default);
}

public interface IStorage;