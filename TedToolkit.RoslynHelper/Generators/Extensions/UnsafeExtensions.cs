// -----------------------------------------------------------------------
// <copyright file="UnsafeExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IUnsafe"/>.
/// </summary>
public static class UnsafeExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IUnsafe
#pragma warning restore CA1034
    {
        /// <summary>
        /// Gets <see langword="unsafe"/>.
        /// </summary>
        public TItem Unsafe
        {
            get
            {
                instance.IsUnsafe = true;
                return instance;
            }
        }

        internal void AddUnsafe(ref SourceBuilder builder)
        {
            if (!instance.IsUnsafe)
                return;

            builder.Append("unsafe ");
        }
    }
}