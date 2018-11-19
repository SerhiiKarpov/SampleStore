namespace SampleStore.Data.EF
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class encapsulating ef query materializer.
    /// </summary>
    /// <seealso cref="IQueryMaterializer" />
    public class EfQueryMaterializer : IQueryMaterializer
    {
        #region Methods

        /// <summary>
        /// Counts the entities returned by the specified query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The number of entities returned by the specified query.
        /// </returns>
        public Task<int> Count<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default(CancellationToken))
        {
            return query.CountAsync(cancellationToken);
        }

        /// <summary>
        /// Materializes the specified query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query to materialize.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// The materialized data.
        /// </returns>
        public Task<List<TResult>> ToList<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default(CancellationToken))
        {
            return query.ToListAsync(cancellationToken);
        }

        #endregion Methods
    }
}