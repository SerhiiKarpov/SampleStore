namespace SampleStore.UI.Extensions
{
    using Microsoft.Extensions.DependencyInjection;

    using SampleStore.UI.Configuration;

    /// <summary>
    /// Class encapsulating service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Configures the UI.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void ConfigureUI(this IServiceCollection services)
        {
            services.ConfigureOptions<RclStaticFileOptions>();
        }

        #endregion Methods
    }
}