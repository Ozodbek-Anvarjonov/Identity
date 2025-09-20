using Identity.Persistence.DataContexts;

namespace Identity.Persistence.UnitOfWorks.Interfaces;

public interface IUnitOfWork : ITransactionManager, IDisposable, IAsyncDisposable
{
    AppDbContext DbContext { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
