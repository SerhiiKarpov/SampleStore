namespace SampleStore.Data.EF
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating ef repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="IRepository{TEntity}" />
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region Fields

        /// <summary>
        /// The set
        /// </summary>
        private readonly DbSet<TEntity> _set;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{TEntity}" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="set">The set.</param>
        public EfRepository(IUnitOfWork unitOfWork, DbSet<TEntity> set)
        {
            UnitOfWork = unitOfWork.ThrowIfArgumentIsNull(nameof(unitOfWork));
            _set = set.ThrowIfArgumentIsNull(nameof(set));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the query to get all entities of this type from DB.
        /// </summary>
        /// <value>
        /// The query to get all entities of this type from DB.
        /// </value>
        /// <remarks>
        /// It can be used to build more complex queries.
        /// </remarks>
        public IQueryable<TEntity> Query
        {
            get
            {
                return _set;
            }
        }

        /// <summary>
        /// Gets the unit of work that issued this repository.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the specified entities to this repository.
        /// </summary>
        /// <param name="entities">The entities to add to this repository.</param>
        public void Add(IEnumerable<TEntity> entities)
        {
            _set.AddRange(entities);
        }

        /// <summary>
        /// Removes the specified entities from this repository.
        /// </summary>
        /// <param name="entities">The entities to remove from this repository.</param>
        public void Remove(IEnumerable<TEntity> entities)
        {
            _set.RemoveRange(entities);
        }

        #endregion Methods
    }
}