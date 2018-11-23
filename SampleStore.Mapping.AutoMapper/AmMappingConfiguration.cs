namespace SampleStore.Mapping.AutoMapper
{
    using System;

    using global::AutoMapper;

    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating am mapping configuration.
    /// </summary>
    /// <seealso cref="IMappingConfiguration" />
    public class AmMappingConfiguration : IMappingConfiguration
    {
        #region Fields

        /// <summary>
        /// The expression
        /// </summary>
        private readonly IMapperConfigurationExpression _expression;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmMappingConfiguration"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public AmMappingConfiguration(IMapperConfigurationExpression expression)
        {
            _expression = expression.ThrowIfArgumentIsNull(nameof(expression));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Configures the specified create mapping.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="createMapping">The create mapping.</param>
        /// <returns>
        /// The <seealso cref="IMappingConfiguration"/>.
        /// </returns>
        public IMappingConfiguration Configure<TSource, TTarget>(Action<IMapping<TSource, TTarget>> createMapping)
        {
            createMapping.ThrowIfArgumentIsNull(nameof(createMapping));

            var mappingExpression = _expression.CreateMap<TSource, TTarget>();
            createMapping(new AmMapping<TSource, TTarget>(mappingExpression));
            return this;
        }

        #endregion Methods
    }
}