namespace SampleStore.Data.EF
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating ef unit of work.
    /// </summary>
    /// <seealso cref="IUnitOfWork" />
    public class EfUnitOfWork : IUnitOfWork
    {
        #region Fields

        /// <summary>
        /// The context
        /// </summary>
        private readonly DbContext _context;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public EfUnitOfWork(DbContext context)
        {
            _context = context.ThrowIfArgumentIsNull(nameof(context));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// The repository.
        /// </returns>
        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            return new EfRepository<TEntity>(this, _context.Set<TEntity>());
        }

        /// <summary>
        /// Saves the changes in repositories returned by this instance.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task object.
        /// </returns>
        public async Task SaveChanges(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException x)
            {
                var conflictedEntities = x.Entries.Select(entry => entry.Entity);
                throw new ConcurrencyException(conflictedEntities);
            }
        }

        #endregion Methods
    }
}