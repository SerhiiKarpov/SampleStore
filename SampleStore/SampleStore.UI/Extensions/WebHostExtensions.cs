namespace SampleStore.UI.Extensions
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Seed;

    /// <summary>
    /// Class encapsulating web host extensions.
    /// </summary>
    public static class WebHostExtensions
    {
        #region Fields

        /// <summary>
        /// The default password key
        /// </summary>
        private const string SeederPasswordKey = "SeederPassword";

        /// <summary>
        /// The seeding error message
        /// </summary>
        private const string SeedingErrorMessage = "An error occurred creating the DB.";

        /// <summary>
        /// The super admin prototype section
        /// </summary>
        private const string SuperAdminPrototypeSection = "SuperAdminPrototype";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Ensures that the DB is seeded.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns>The passed host to enable method call chaining.</returns>
        public static IWebHost EnsureSeeded(this IWebHost host)
        {
            DoEnsureSeeded(host).GetAwaiter().GetResult();
            return host;
        }

        /// <summary>
        /// Does the ensure seeded.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task DoEnsureSeeded(IWebHost host)
        {
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var superAdminPrototype = configuration.GetSection(SuperAdminPrototypeSection).Get<User>();
                    var seederPassword = configuration[SeederPasswordKey];

                    var seeder = services.GetRequiredService<DatabaseSeeder>();
                    var needsSeeding = await seeder.NeedsSeeding();
                    if (needsSeeding)
                    {
                        await seeder.Seed(superAdminPrototype, seederPassword);
                    }
                }
                catch (Exception x)
                {
                    var logger = services.GetRequiredService<ILogger<DatabaseSeeder>>();
                    logger.LogError(x, SeedingErrorMessage);
                }
            }
        }

        #endregion Methods
    }
}