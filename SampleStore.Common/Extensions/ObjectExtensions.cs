using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SampleStore.Common.Extensions;

public static class ObjectExtensions
{
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

    public static TObject ThrowIfArgumentIsNull<TObject>(this TObject argument, string name)
        where TObject : class
    {
        if (argument == null)
        {
            throw new ArgumentNullException(name);
        }

        return argument;
    }

    public static IEnumerable<TObject> ToEnumerable<TObject>(this TObject item)
    {
        yield return item;
    }
}