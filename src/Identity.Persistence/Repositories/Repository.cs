using Identity.Domain.Common.Entities;
using Identity.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Identity.Persistence.Repositories;

public class Repository<TEntity>(DbContext context) : IRepository<TEntity>
    where TEntity : Entity
{
    public IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? predicate = default,
        bool asNoTracking = true,
        string[]? includes = default
        )
    {
        var query = context.Set<TEntity>().AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (predicate is not null)
            query = query.Where(predicate);

        query = query.ApplyInclude(includes);

        return query;
    }

    public async Task<TEntity?> GetByIdAsync(
        long id,
        bool asNoTracking = true,
        string[]? includes = default,
        CancellationToken cancellationToken = default
        )
    {
        var query = context.Set<TEntity>().AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        query = query.ApplyInclude(includes);

        var entity = await query.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        return entity;
    }

    public async Task<long> CreateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);

        if (saveChanges)
            await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        context.Update(entity);

        if (saveChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        context.Remove(entity);

        if (saveChanges)
            await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);
}
