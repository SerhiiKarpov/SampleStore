namespace SampleStore.Mapping
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// An interface for mapping.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public interface IMapping<TSource, TTarget>
    {
        #region Methods

        /// <summary>
        /// Ignores the specified target member.
        /// </summary>
        /// <typeparam name="TTargetMember">The type of the target member.</typeparam>
        /// <param name="targetMember">The target member.</param>
        /// <returns>
        /// The <seealso cref="IMapping{TSource, TTarget}"/>.
        /// </returns>
        IMapping<TSource, TTarget> Ignore<TTargetMember>(Expression<Func<TTarget, TTargetMember>> targetMember);

        /// <summary>
        /// Maps the specified target member.
        /// </summary>
        /// <typeparam name="TTargetMember">The type of the target member.</typeparam>
        /// <param name="targetMember">The target member.</param>
        /// <param name="sourceMember">The source member.</param>
        /// <returns>
        /// The <seealso cref="IMapping{TSource, TTarget}"/>.
        /// </returns>
        IMapping<TSource, TTarget> Map<TTargetMember>(
            Expression<Func<TTarget, TTargetMember>> targetMember,
            Expression<Func<TSource, TTargetMember>> sourceMember);

        /// <summary>
        /// Maps all members.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// The <seealso cref="IMapping{TSource, TTarget}"/>.
        /// </returns>
        IMapping<TSource, TTarget> MapAll(Expression<Func<TSource, TTarget>> expression);

        #endregion Methods
    }
}