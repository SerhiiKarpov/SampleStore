using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SampleStore.Common.Helpers;

public static class PropertyHelper
{
    public static PropertyInfo GetProperty<TObject, TProperty>(Expression<Func<TObject, TProperty>> selector)
    {
        return ((selector.Body as MemberExpression)?.Member as PropertyInfo) ?? throw new ArgumentException("Invalid selector.", nameof(selector));
    }
}