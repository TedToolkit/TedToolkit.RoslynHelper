// -----------------------------------------------------------------------
// <copyright file="AttributesExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IAttributes"/>
/// </summary>
public static class AttributesExtensions
{
#pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IAttributes
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add attribute
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <returns>the item</returns>
        public ref TItem AddAttribute(Attribute attribute)
        {
            instance.Attributes.Add(attribute);
            return ref instance;
        }

        internal void AddAttributes(ref SourceBuilder builder)
        {
            if (instance.Attributes.Count == 0)
                return;

            foreach (var attribute in instance.Attributes.AsSpan())
            {
                builder.Append('[');
                attribute.ToCode(ref builder);
                builder.Append(']');
                builder.AppendLine();
            }
        }
    }
}