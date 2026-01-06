// -----------------------------------------------------------------------
// <copyright file="GeneralExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

using Cysharp.Text;

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

    extension(string value)
#pragma warning disable S2325
    {
        /// <summary>
        /// To the argument name
        /// </summary>
        /// <returns>argument Name</returns>
        public string ToArgumentName()
            => ZString.Concat('@', value);
    }
#pragma warning restore S2325

    extension(Type value)
#pragma warning disable S2325
    {
        /// <summary>
        /// Get the tool name
        /// </summary>
        /// <returns>tool name</returns>
        public string GetToolName()
        {
            var builder = new SourceBuilder();

            try
            {
                value.ToExpression().ToCode(ref builder);
                return builder.ToCode();
            }
            finally
            {
                builder.Dispose();
            }
        }

        /// <summary>
        ///  Get the version
        /// </summary>
        /// <returns>version</returns>
        public string GetVersion()
            => value.Assembly.GetName().Version.ToString();
    }
#pragma warning restore S2325
}