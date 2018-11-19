namespace SampleStore.Data.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using SampleStore.Common;
    using SampleStore.Data.Entities;

    /// <summary>
    /// Class encapsulating entity repository extensions.
    /// </summary>
    public static class EntityRepositoryExtensions
    {
        #region Methods

        /// <summary>
        /// Finds an entity using the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A found entity.
        /// </returns>
        public static async Task<TEntity> Find<TEntity>(
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

        /// <summary>
        /// Finds and entity by the identifier asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The found entity.
        /// </returns>
        public static Task<TEntity> FindById<TEntity>(
            this IRepository<TEntity> repository,
            Guid id,
            IQueryMaterializer queryMaterializer,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : Entity
        {
            cancellationToken.ThrowIfCancellationRequested();
            return repository.Find(e => e.Id == id, queryMaterializer, cancellationToken);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task object.</returns>
        public static async Task Update<TEntity>(
            this IRepository<TEntity> repository,
            TEntity entity,
            IQueryMaterializer queryMaterializer,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : Entity
        {
            cancellationToken.ThrowIfCancellationRequested();
            repository.ThrowIfArgumentIsNull(nameof(repository));

            var query = repository.Query.Where(e => e.Id == entity.Id);
            var found = await queryMaterializer.FirstOrDefault(query, cancellationToken);
            if (found == null)
            {
                return;
            }

            entity.CopyTo(found);
            await repository.UnitOfWork.SaveChanges(cancellationToken);
        }

        #endregion Methods
    }
}