namespace SampleStore.Mapping
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// An interface for mapping.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <remarks>
    /// This interface represents convention-based mapping.
    /// If you want to leave target properties alone with their default values, use Ignore method.
    /// If you want to specify custom mapping logic for some target property, use Map method.
    /// Please note, that it is expected that properties with the same name in source and target type will be bound automatically.
    /// Please note, that if types of source member and value returned by target expression are different, then the mapper implementation must attempt
    /// to convert types using either simply type conversions or other registered mappings within mapping configuration.
    /// If you want to specify custom construction logic to target object, use Construct method.
    /// </remarks>
    public interface IMapping<TSource, TTarget>
    {
        #region Methods

        /// <summary>
        /// Constructs a target object using the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// The <seealso cref="IMapping{TSource, TTarget}"/>.
        /// </returns>
        IMapping<TSource, TTarget> Construct(Expression<Func<TSource, TTarget>> expression);

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
        /// <typeparam name="TSourceMember">The type of the source member.</typeparam>
        /// <typeparam name="TTargetMember">The type of the target member.</typeparam>
        /// <param name="targetMember">The target member.</param>
        /// <param name="sourceMember">The source member.</param>
        /// <returns>
        /// The <seealso cref="IMapping{TSource, TTarget}" />.
        /// </returns>
        IMapping<TSource, TTarget> Map<TSourceMember, TTargetMember>(
            Expression<Func<TTarget, TTargetMember>> targetMember,
            Expression<Func<TSource, TSourceMember>> sourceMember);

        #endregion Methods
    }
}