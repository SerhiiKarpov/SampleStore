namespace SampleStore.Mapping
{
    /// <summary>
    /// An interface for mapper provider.
    /// </summary>
    public interface IMapperProvider
    {
        #region Methods

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TTarget">The type of the target object.</typeparam>
        /// <returns>The mapper.</returns>
        IMapper<TSource, TTarget> GetMapper<TSource, TTarget>();

        #endregion Methods
    }
}