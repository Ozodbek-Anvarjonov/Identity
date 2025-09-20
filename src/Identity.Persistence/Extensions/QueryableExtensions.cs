using Identity.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplyInclude<TEntity>(this IQueryable<TEntity> src, string[]? includes)
        where TEntity : class
    {
        if (includes is not null)
            foreach (var include in includes)
                src = src.Include(include);

        return src;
    }

    public static IQueryable<TEntity> ApplySoftDeleteFilter<TEntity>(IQueryable<TEntity> query)
        where TEntity : ISoftDeletedEntity
    {
        return query.Where(entity => entity.IsDeleted == false);
    }
}
