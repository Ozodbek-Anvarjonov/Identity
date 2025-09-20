using Identity.Persistence.DataContexts;
using Identity.Persistence.UnitOfWorks.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Persistence.UnitOfWorks;

public class UnitOfWork(
    AppDbContext dbContext
    ) : IUnitOfWork
{
    #region Unit of work
    private IDbContextTransaction? currentTransaction;
    private bool disposed;

    public bool HasActiveTransaction => currentTransaction != null;

    public AppDbContext DbContext => dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);

    #region Transaction Management
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (HasActiveTransaction)
            return;

        currentTransaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (currentTransaction is null)
            throw new InvalidOperationException("There is no active transaction to commit.");

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            await currentTransaction.CommitAsync(cancellationToken);
            await DisposeCurrentTransactionAsync();
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (currentTransaction is null)
            return;

        await currentTransaction.RollbackAsync(cancellationToken);
        await DisposeCurrentTransactionAsync();
    }

    private async Task DisposeCurrentTransactionAsync()
    {
        await currentTransaction!.DisposeAsync();
        currentTransaction = null;
    }
    #endregion

    #region Disposal
    public void Dispose()
    {
        if (disposed) return;

        dbContext.Dispose();
        currentTransaction?.Dispose();
        disposed = true;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (disposed) return;

        await dbContext.DisposeAsync();
        if (HasActiveTransaction)
            await currentTransaction!.DisposeAsync();
        disposed = true;
        GC.SuppressFinalize(this);
    }
    #endregion
    #endregion
}
