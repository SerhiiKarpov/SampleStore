namespace SampleStore.Mapping
{
    using System;

    /// <summary>
    /// An interface for mapping configuration.
    /// </summary>
    public interface IMappingConfiguration
    {
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
        IMappingConfiguration Configure<TSource, TTarget>(Action<IMapping<TSource, TTarget>> createMapping);

        #endregion Methods
    }
}