namespace SampleStore.Mapping.AutoMapper.Extensions
{
    using System;

    using global::AutoMapper;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Class encapsulating service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the automatic mapper.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configure">The configure.</param>
        public static void AddAutoMapper(this IServiceCollection services, Action<IMappingConfiguration> configure)
        {
            var configuration = new MapperConfiguration(ce => configure(new AmMappingConfiguration(ce)));
            configuration.AssertConfigurationIsValid();
            var mapperProvider = new AmMapperProvider(configuration);
            services.AddSingleton<IMapperProvider>(mapperProvider);
        }

        #endregion Methods
    }
}