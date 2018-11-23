namespace SampleStore.Data.Seed.Extensions
{
    using Microsoft.Extensions.DependencyInjection;

    using SampleStore.Common.Extensions;
    using SampleStore.Data.Seed.Commands;

    /// <summary>
    /// Class encapsulating service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the database seeder.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddDatabaseSeeder(this IServiceCollection services)
        {
            services.ThrowIfArgumentIsNull(nameof(services));

            services.AddScoped<DatabaseSeeder>();
            services.AddTransient<ICreateSuperAdminCommandFactory, SeederCommandFactory>();
            services.AddTransient<ICreateRolesCommandFactory, SeederCommandFactory>();
            services.AddTransient<IAddUserToRolesCommandFactory, SeederCommandFactory>();
        }

        #endregion Methods
    }
}