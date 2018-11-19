namespace SampleStore.Data
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An interface for repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        #region Properties

        /// <summary>
        /// Gets the query to get all entities of this type from DB.
        /// </summary>
        /// <value>
        /// The query to get all entities of this type from DB.
        /// </value>
        /// <remarks>It can be used to build more complex queries.</remarks>
        IQueryable<TEntity> Query
        {
            get;
        }

        /// <summary>
        /// Gets the unit of work that issued this repository.
        /// </summary>
        IUnitOfWork UnitOfWork
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the specified entities to this repository.
        /// </summary>
        /// <param name="entities">The entities to add to this repository.</param>
        void Add(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes the specified entities from this repository.
        /// </summary>
        /// <param name="entities">The entities to remove from this repository.</param>
        void Remove(IEnumerable<TEntity> entities);

        #endregion Methods
    }
}