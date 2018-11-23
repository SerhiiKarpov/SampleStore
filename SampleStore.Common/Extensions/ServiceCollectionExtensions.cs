namespace SampleStore.Common.Extensions
{
    using Microsoft.Extensions.DependencyInjection;

    using SampleStore.Common.Services;

    /// <summary>
    /// Class encapsulating service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the common services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>
        /// The services parameter to chain calls.
        /// </returns>
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTime, DateTimeService>();

            return services;
        }

        #endregion Methods
    }
}