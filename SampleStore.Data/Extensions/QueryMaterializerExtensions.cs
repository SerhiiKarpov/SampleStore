using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Data.Extensions;

public static class QueryMaterializerExtensions
{
    public static async Task<bool> Any<TResult>(
        this IQueryMaterializer queryMaterializer,
        IQueryable<TResult> query,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(queryMaterializer);
        ArgumentNullException.ThrowIfNull(query);

        var count = await queryMaterializer.Count(query, cancellationToken);
        var any = count != 0;
        return any;
    }

    public static async Task<TResult> First<TResult>(
        this IQueryMaterializer queryMaterializer,
        IQueryable<TResult> query,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(queryMaterializer);
        ArgumentNullException.ThrowIfNull(query);

        var items = await queryMaterializer.ToList(query.Take(1), cancellationToken);
        return items.First();
    }

    public static async Task<TResult?> FirstOrDefault<TResult>(
        this IQueryMaterializer queryMaterializer,
        IQueryable<TResult> query,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(queryMaterializer);
        ArgumentNullException.ThrowIfNull(query);

        var items = await queryMaterializer.ToList(query.Take(1), cancellationToken);
        return items.FirstOrDefault();
    }

    public static async Task<TResult> Single<TResult>(
        this IQueryMaterializer queryMaterializer,
        IQueryable<TResult> query,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(queryMaterializer);
        ArgumentNullException.ThrowIfNull(query);

        var items = await queryMaterializer.ToList(query.Take(2), cancellationToken);
        return items.Single();
    }

    public static async Task<TResult?> SingleOrDefault<TResult>(
        this IQueryMaterializer queryMaterializer,
        IQueryable<TResult> query,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(queryMaterializer);
        ArgumentNullException.ThrowIfNull(query);

        var items = await queryMaterializer.ToList(query.Take(2), cancellationToken);
        return items.SingleOrDefault();
    }
}