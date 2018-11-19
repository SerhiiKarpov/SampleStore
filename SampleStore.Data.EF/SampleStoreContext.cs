namespace SampleStore.Data.EF
{
    using Microsoft.EntityFrameworkCore;

    using SampleStore.Data.EF.Extensions;

    /// <summary>
    /// Class encapsulating application database context.
    /// </summary>
    internal class SampleStoreContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleStoreContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SampleStoreContext(DbContextOptions<SampleStoreContext> options)
            : base(options)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder
                .BuildIdentityModel()
                .BuildDomainModel();
        }

        #endregion Methods
    }
}