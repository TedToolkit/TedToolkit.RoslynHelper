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
    /// <summary>
    /// Indent the string
    /// </summary>
    /// <param name="str">the target string</param>
    /// <returns>result</returns>
    public static string Indent(this string str)
        => "\n\t" + str.Replace("\n", "\n\t");

    private static class ArrayAccessor<T>
    {
        public static readonly FieldInfo ItemsField = typeof(List<T>)
            .GetRuntimeField("_items")!;
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