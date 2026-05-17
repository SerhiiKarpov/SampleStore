using System.Collections.Generic;

namespace SampleStore.Common.Extensions;

public static class CollectionExtensions
{
    public static void Add<TItem>(this ICollection<TItem> collection, params TItem[] items)
    {
        collection.ThrowIfArgumentIsNull(nameof(collection));
        items.ThrowIfArgumentIsNull(nameof(items));

        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}