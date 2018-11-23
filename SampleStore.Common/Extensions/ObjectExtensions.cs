namespace SampleStore.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Class encapsulating object extensions.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Methods

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void CopyTo<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : TTarget
        {
            var properties =
                from property in typeof(TTarget).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                where property.CanRead && property.CanWrite
                select property;
            foreach (var property in properties)
            {
                var value = property.GetValue(source);
                property.SetValue(target, value);
            }
        }

        /// <summary>
        /// Throws if argument is null.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="name">The name.</param>
        /// <returns>The passed argument to chain calls.</returns>
        /// <exception cref="ArgumentNullException">If argument is null.</exception>
        public static TObject ThrowIfArgumentIsNull<TObject>(this TObject argument, string name)
            where TObject : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }

            return argument;
        }

        /// <summary>
        /// Makes a one item enumerable from the specified item.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="IEnumerable{TObject}"/>.</returns>
        public static IEnumerable<TObject> ToEnumerable<TObject>(this TObject item)
        {
            yield return item;
        }

        #endregion Methods
    }
}