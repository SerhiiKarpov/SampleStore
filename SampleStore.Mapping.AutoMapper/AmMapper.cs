namespace SampleStore.Mapping.AutoMapper
{
    using global::AutoMapper;

    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating am mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <seealso cref="IMapper{TSource, TTarget}" />
    public class AmMapper<TSource, TTarget> : IMapper<TSource, TTarget>
    {
        #region Fields

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmMapper{TSource, TTarget}"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public AmMapper(IMapper mapper)
        {
            _mapper = mapper.ThrowIfArgumentIsNull(nameof(mapper));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        /// The mapped object.
        /// </returns>
        public TTarget Map(TSource source)
        {
            return _mapper.Map<TSource, TTarget>(source);
        }

        #endregion Methods
    }
}