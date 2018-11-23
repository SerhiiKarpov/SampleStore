namespace SampleStore.Data.EF.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the entity framework data access.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionString">The connection string.</param>
        public static void AddEntityFrameworkDataAccess(this IServiceCollection services, string connectionString)
        {
            services.ThrowIfArgumentIsNull(nameof(services));

            services.AddDbContext<DbContext, SampleStoreContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddSingleton<IQueryMaterializer, EfQueryMaterializer>();
        }

        #endregion Methods
    }
}