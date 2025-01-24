using Iliami.Identity.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Iliami.Identity.Persistence;

public class UnitOfWork(IServiceProvider serviceProvider) : IUnitOfWork
{
    public async Task<IUnitOfWorkScope> StartScope(CancellationToken ct = default)
    {
        var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var transaction = await dbContext.Database.BeginTransactionAsync(ct);
        return new UnitOfWorkScope(scope, transaction);
    }
}

public class UnitOfWorkScope(
    IServiceScope scope,
    IDbContextTransaction transaction) : IUnitOfWorkScope
{
    public TStorage GetStorage<TStorage>() where TStorage : IStorage =>
        scope.ServiceProvider.GetRequiredService<TStorage>();

    public Task Commit(CancellationToken ct = default) =>
        transaction.CommitAsync(ct);

    public async ValueTask DisposeAsync()
    {
        await transaction.DisposeAsync();

        if (scope is IAsyncDisposable scopeAsyncDisposable)
        {
            await scopeAsyncDisposable.DisposeAsync();
        }
        else
        {
            scope.Dispose();
        }
    }
}