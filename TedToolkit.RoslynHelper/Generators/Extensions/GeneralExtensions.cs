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
#pragma warning disable RCS1139, RCS1263
    /// <param name="str">the target string</param>
    extension(string str)
#pragma warning restore RCS1139, RCS1263
#pragma warning disable S2325
    {
        /// <summary>
        /// Indent the string
        /// </summary>
        /// <returns>result</returns>
        public string Indent()
            => str.AddLeading("\t");

        /// <summary>
        /// To the summary
        /// </summary>
        /// <returns>result string</returns>
        public string ToSummary()
            => str.AddLeading("/// ");

        private string AddLeading(string leading)
            => leading + str.Replace("\n", "\n" + leading);
    }
#pragma warning restore S2325

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