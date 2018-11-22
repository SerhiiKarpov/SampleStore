namespace SampleStore.Mapping.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class encapsulating mapper extensions.
    /// </summary>
    public static class MapperExtensions
    {
        #region Methods

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source.</param>
        /// <returns>
        /// The <seealso cref="IEnumerable{TTarget}"/>.
        /// </returns>
        public static IEnumerable<TTarget> Map<TSource, TTarget>(this IMapper<TSource, TTarget> mapper, IEnumerable<TSource> source)
        {
            return source.Select(mapper.Map);
        }

        #endregion Methods
    }
}