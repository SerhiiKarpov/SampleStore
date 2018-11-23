namespace SampleStore.Data.Extensions
{
    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating repository extensions.
    /// </summary>
    public static class RepositoryExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="entity">The entity.</param>
        public static void Add<TEntity>(this IRepository<TEntity> repository, TEntity entity)
            where TEntity : class
        {
            repository.ThrowIfArgumentIsNull(nameof(repository));
            entity.ThrowIfArgumentIsNull(nameof(entity));
            repository.Add(entity.ToEnumerable());
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="entity">The entity.</param>
        public static void Remove<TEntity>(this IRepository<TEntity> repository, TEntity entity)
            where TEntity : class
        {
            if (entity == null)
            {
                return;
            }

            repository.ThrowIfArgumentIsNull(nameof(repository));
            repository.Remove(entity.ToEnumerable());
        }

        #endregion Methods
    }
}