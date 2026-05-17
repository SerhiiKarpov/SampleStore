using System.Collections.Generic;
using System.Linq;

namespace SampleStore.Data;

public interface IRepository<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> Query { get; }

    void Add(IEnumerable<TEntity> entities);

    void Remove(IEnumerable<TEntity> entities);
}