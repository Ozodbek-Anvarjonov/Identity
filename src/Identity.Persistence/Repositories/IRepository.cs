using Identity.Domain.Common.Entities;
using Identity.Domain.Enums;
using System.Linq.Expressions;

namespace Identity.Persistence.Repositories;

public interface IRepository<TEntity>
    where TEntity : IEntity
{
    IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? predicate = default,
        bool asNoTracking = true,
        DeletedState deletedState = DeletedState.Active,
        string[]? includes = default
        );

    Task<TEntity?> GetByIdAsync(
        long id,
        bool asNoTracking = true,
        DeletedState deletedState = DeletedState.Active,
        string[]? includes = default,
        CancellationToken cancellationToken = default
        );

    Task<long> CreateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
