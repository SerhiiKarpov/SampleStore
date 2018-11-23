namespace SampleStore.Mapping.AutoMapper
{
    using System;

    using global::AutoMapper;

    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating am mapper provider.
    /// </summary>
    /// <seealso cref="IMapperProvider" />
    public class AmMapperProvider : IMapperProvider
    {
        #region Fields

        /// <summary>
        /// The lazy mapper
        /// </summary>
        private readonly Lazy<IMapper> _lazyMapper;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmMapperProvider"/> class.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        public AmMapperProvider(IConfigurationProvider configurationProvider)
        {
            configurationProvider.ThrowIfArgumentIsNull(nameof(configurationProvider));
            _lazyMapper = new Lazy<IMapper>(() => configurationProvider.CreateMapper());
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TTarget">The type of the target object.</typeparam>
        /// <returns>
        /// The mapper.
        /// </returns>
        public IMapper<TSource, TTarget> GetMapper<TSource, TTarget>()
        {
            return new AmMapper<TSource, TTarget>(_lazyMapper.Value);
        }

        #endregion Methods
    }
}