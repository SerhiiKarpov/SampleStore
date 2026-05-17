using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities;

namespace SampleStore.Data.Extensions;

public static class RepositoryExtensions
{
    public static async Task<TEntity?> Find<TEntity>(
        this IRepository<TEntity> repository,
        Expression<Func<TEntity, bool>> predicate,
        IQueryMaterializer queryMaterializer,
        CancellationToken cancellationToken = default(CancellationToken))
        where TEntity : Entity
    {
        cancellationToken.ThrowIfCancellationRequested();
        repository.ThrowIfArgumentIsNull(nameof(repository));
        predicate.ThrowIfArgumentIsNull(nameof(predicate));
        queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));

        var query = repository.Query.Where(predicate);
        var found = await queryMaterializer.FirstOrDefault(query, cancellationToken);
        return found;
    }

    public static Task<TEntity?> FindById<TEntity>(
        this IRepository<TEntity> repository,
        Guid id,
        IQueryMaterializer queryMaterializer,
        CancellationToken cancellationToken = default(CancellationToken))
        where TEntity : Entity
    {
        cancellationToken.ThrowIfCancellationRequested();
        return repository.Find(e => e.Id == id, queryMaterializer, cancellationToken);
    }
}