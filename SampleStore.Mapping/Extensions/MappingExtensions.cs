namespace SampleStore.Mapping.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Class encapsulating mapping extensions.
    /// </summary>
    public static class MappingExtensions
    {
        #region Fields

        /// <summary>
        /// The lambda method
        /// </summary>
        private static readonly MethodInfo LambdaMethod = ((MethodCallExpression)((Expression<Func<Expression<Func<object>>>>)(() => Expression.Lambda<Func<object>>(null))).Body).Method.GetGenericMethodDefinition();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Maps all writeable public properties of target type from readable public property with the same name of source type, if exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapping">The mapping.</param>
        /// <returns>
        /// The <seealso cref="IMapping{TSource, TTarget}"/>.
        /// </returns>
        public static IMapping<TSource, TTarget> MapAllWithTheSameName<TSource, TTarget>(this IMapping<TSource, TTarget> mapping)
        {
            // TODO: This method is too complex and is likely to have bugs, refactor it.
            var targetProperties = typeof(TTarget).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.CanWrite);
            foreach (var targetProperty in targetProperties)
            {
                var sourceProperty = typeof(TSource).GetProperty(targetProperty.Name, BindingFlags.Public | BindingFlags.Instance);
                if (sourceProperty == null
                    || !sourceProperty.CanRead
                    || !targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                {
                    continue;
                }

                var targetParameter = Expression.Parameter(typeof(TTarget));
                var targetLambdaMethod = LambdaMethod.MakeGenericMethod(typeof(Func<,>).MakeGenericType(typeof(TTarget), targetProperty.PropertyType));
                var targetExpression = targetLambdaMethod.Invoke(
                    null,
                    new object[]
                    {
                        Expression.MakeMemberAccess(targetParameter, targetProperty),
                        new[] { targetParameter }
                    });

                var sourceParameter = Expression.Parameter(typeof(TSource));
                var sourceLambdaMethod = LambdaMethod.MakeGenericMethod(typeof(Func<,>).MakeGenericType(typeof(TSource), targetProperty.PropertyType));
                var sourceExpression = sourceLambdaMethod.Invoke(
                    null,
                    new object[]
                    {
                        Expression.MakeMemberAccess(sourceParameter, sourceProperty),
                        new[] { sourceParameter }
                    });

                var mapMethod = GetMapMethod<TSource, TTarget>().MakeGenericMethod(targetProperty.PropertyType);
                mapMethod.Invoke(mapping, new object[] { targetExpression, sourceExpression });
            }

            return mapping;
        }

        /// <summary>
        /// Gets the map method.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <returns>
        /// The map method.
        /// </returns>
        private static MethodInfo GetMapMethod<TSource, TTarget>()
        {
            Expression<Func<IMapping<TSource, TTarget>, IMapping<TSource, TTarget>>> mapExpression = x => x.Map<object>(null, null);
            var mapMethod = ((MethodCallExpression)mapExpression.Body).Method.GetGenericMethodDefinition();
            return mapMethod;
        }

        #endregion Methods
    }
}