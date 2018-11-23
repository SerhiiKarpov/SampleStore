namespace SampleStore.Data.Extensions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating query materializer extensions.
    /// </summary>
    public static class QueryMaterializerExtensions
    {
        #region Methods

        /// <summary>
        /// Checks if the specified query returns at least one item.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><c>true</c> if the specified query returns at least one item, <c>false</c> otherwise.</returns>
        public static async Task<bool> Any<TResult>(
            this IQueryMaterializer queryMaterializer,
            IQueryable<TResult> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
            query.ThrowIfArgumentIsNull(nameof(query));

            var count = await queryMaterializer.Count(query, cancellationToken);
            var any = count != 0;
            return any;
        }

        /// <summary>
        /// Returns a first item from the specified query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A first item.</returns>
        public static async Task<TResult> First<TResult>(
            this IQueryMaterializer queryMaterializer,
            IQueryable<TResult> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
            query.ThrowIfArgumentIsNull(nameof(query));

            var items = await queryMaterializer.ToList(query.Take(1), cancellationToken);
            return items.First();
        }

        /// <summary>
        /// Returns a first item from the specified query or default value of item type if query doesn't return any item.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A first item or default.</returns>
        public static async Task<TResult> FirstOrDefault<TResult>(
            this IQueryMaterializer queryMaterializer,
            IQueryable<TResult> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
            query.ThrowIfArgumentIsNull(nameof(query));

            var items = await queryMaterializer.ToList(query.Take(1), cancellationToken);
            return items.FirstOrDefault();
        }

        /// <summary>
        /// Returns a signle item from the specified query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A signle item from the specified query.</returns>
        public static async Task<TResult> Single<TResult>(
            this IQueryMaterializer queryMaterializer,
            IQueryable<TResult> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
            query.ThrowIfArgumentIsNull(nameof(query));

            var items = await queryMaterializer.ToList(query.Take(2), cancellationToken);
            return items.Single();
        }

        /// <summary>
        /// Returns a signle item from the specified query or default value of item type if query doesn't return any item.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A signle item from the specified query or default value of item type if query doesn't return any item.</returns>
        public static async Task<TResult> SingleOrDefault<TResult>(
            this IQueryMaterializer queryMaterializer,
            IQueryable<TResult> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
            query.ThrowIfArgumentIsNull(nameof(query));

            var items = await queryMaterializer.ToList(query.Take(2), cancellationToken);
            return items.SingleOrDefault();
        }

        #endregion Methods
    }
}