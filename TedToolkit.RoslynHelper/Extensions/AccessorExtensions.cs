// -----------------------------------------------------------------------
// <copyright file="AccessorExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The extensions for the <see cref="IAccessors"/>.
/// </summary>
public static class AccessorExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IAccessors
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add attribute.
        /// </summary>
        /// <param name="attribute">attribute.</param>
        /// <returns>the item.</returns>
        public TItem AddAccessor(Accessor attribute)
        {
            instance.Accessors.Add(attribute);
            return instance;
        }

        internal void AddAccessors(ref SourceBuilder builder)
        {
            if (instance.Accessors.Count == 0)
            {
                builder.Append(';');
                return;
            }

            builder.BeginBlock();
            foreach (var attribute in instance.Accessors.AsSpan())
            {
                builder.AppendLine();
                attribute.ToCode(ref builder);
            }

            builder.EndBlock();
        }
    }
}