using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using SampleStore.Common.Extensions;

namespace SampleStore.Data.EF;

public class EfRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _set;

    public EfRepository(DbSet<TEntity> set)
    {
        _set = set.ThrowIfArgumentIsNull(nameof(set));
    }

    public IQueryable<TEntity> Query => _set;

    public void Add(IEnumerable<TEntity> entities)
    {
        _set.AddRange(entities);
    }

    public void Remove(IEnumerable<TEntity> entities)
    {
        _set.RemoveRange(entities);
    }
}