namespace SampleStore.Mapping
{
    /// <summary>
    /// An interface for mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public interface IMapper<TSource, TTarget>
    {
        #region Methods

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The mapped object.</returns>
        TTarget Map(TSource source);

        #endregion Methods
    }
}