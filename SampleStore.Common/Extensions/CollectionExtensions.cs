namespace SampleStore.Common.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Class encapsulating collection extensions.
    /// </summary>
    public static class CollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the specified items.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void Add<TItem>(this ICollection<TItem> collection, params TItem[] items)
        {
            collection.ThrowIfArgumentIsNull(nameof(collection));
            items.ThrowIfArgumentIsNull(nameof(items));

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        #endregion Methods
    }
}