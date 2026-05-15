namespace SampleStore.Mapping.AutoMapper
{
    using System;
    using System.Linq.Expressions;

    using global::AutoMapper;


    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating am mapping.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <seealso cref="IMapping{TSource, TTarget}" />
    public class AmMapping<TSource, TTarget> : IMapping<TSource, TTarget>
    {
        #region Fields

        /// <summary>
        /// The expression
        /// </summary>
        private readonly IMappingExpression<TSource, TTarget> _expression;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmMapping{TSource, TTarget}"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public AmMapping(IMappingExpression<TSource, TTarget> expression)
        {
            _expression = expression.ThrowIfArgumentIsNull(nameof(expression));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Constructs a target object using the specified factory function.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>
        /// The <seealso cref="T:SampleStore.Mapping.IMapping`2" />.
        /// </returns>
        public IMapping<TSource, TTarget> Construct(Func<TSource, TTarget> factory)
        {
            factory.ThrowIfArgumentIsNull(nameof(factory));

            _expression.ConstructUsing((src, _) => factory(src));
            return this;
        }

        /// <summary>
        /// Ignores the specified target member.
        /// </summary>
        /// <typeparam name="TTargetMember">The type of the target member.</typeparam>
        /// <param name="targetMember">The target member.</param>
        /// <returns>
        /// The <seealso cref="IMapping{TSource, TTarget}"/>.
        /// </returns>
        public IMapping<TSource, TTarget> Ignore<TTargetMember>(Expression<Func<TTarget, TTargetMember>> targetMember)
        {
            targetMember.ThrowIfArgumentIsNull(nameof(targetMember));

            _expression.ForMember(targetMember, x => x.Ignore());
            return this;
        }

        /// <summary>
        /// Maps the specified target member.
        /// </summary>
        /// <typeparam name="TSourceMember">The type of the source member.</typeparam>
        /// <typeparam name="TTargetMember">The type of the target member.</typeparam>
        /// <param name="targetMember">The target member.</param>
        /// <param name="sourceMember">The source member.</param>
        /// <returns>
        /// The <seealso cref="IMapping{TSource, TTarget}"/>.
        /// </returns>
        public IMapping<TSource, TTarget> Map<TSourceMember, TTargetMember>(
            Expression<Func<TTarget, TTargetMember>> targetMember,
            Expression<Func<TSource, TSourceMember>> sourceMember)
        {
            targetMember.ThrowIfArgumentIsNull(nameof(targetMember));
            sourceMember.ThrowIfArgumentIsNull(nameof(sourceMember));

            _expression.ForMember(targetMember, x => x.MapFrom(sourceMember));
            return this;
        }

        #endregion Methods
    }
}