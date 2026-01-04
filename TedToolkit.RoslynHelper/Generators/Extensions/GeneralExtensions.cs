// -----------------------------------------------------------------------
// <copyright file="GeneralExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// Some general things.
/// </summary>
internal static class GeneralExtensions
{
    private static class ArrayAccessor<T>
    {
        public static readonly FieldInfo ItemsField = typeof(List<T>)
#pragma warning disable S3011
            .GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic)!;
#pragma warning restore S3011
    }

    /// <summary>
    /// As span
    /// </summary>
    /// <param name="list">the list</param>
    /// <typeparam name="T">Data</typeparam>
    /// <returns>span</returns>
    public static Span<T> AsSpan<T>(this List<T> list)
    {
        var items = (T[])ArrayAccessor<T>.ItemsField.GetValue(list);
        return new(items, 0, list.Count);
    }
}