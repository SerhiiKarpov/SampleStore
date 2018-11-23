namespace SampleStore.Common.Helpers
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Class encapsulating property helper.
    /// </summary>
    public static class PropertyHelper
    {
        #region Methods

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The property.
        /// </returns>
        public static PropertyInfo GetProperty<TObject, TProperty>(Expression<Func<TObject, TProperty>> selector)
        {
            return (selector.Body as MemberExpression)?.Member as PropertyInfo;
        }

        #endregion Methods
    }
}