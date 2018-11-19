namespace SampleStore.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// An interface for query materializer.
    /// </summary>
    public interface IQueryMaterializer
    {
        #region Methods

        /// <summary>
        /// Counts the entities returned by the specified query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities returned by the specified query.</returns>
        Task<int> Count<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Materializes the specified query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query to materialize.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The materialized data.
        /// </returns>
        Task<List<TResult>> ToList<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default(CancellationToken));

        #endregion Methods
    }
}