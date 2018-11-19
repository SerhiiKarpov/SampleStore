namespace SampleStore.Data
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// An interface for unit of work.
    /// </summary>
    public interface IUnitOfWork
    {
        #region Methods

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Saves the changes in repositories returned by this instance.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task object.
        /// </returns>
        Task SaveChanges(CancellationToken cancellationToken = default(CancellationToken));

        #endregion Methods
    }
}