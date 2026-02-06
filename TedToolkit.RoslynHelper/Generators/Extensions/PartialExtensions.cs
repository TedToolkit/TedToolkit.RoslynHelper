// -----------------------------------------------------------------------
// <copyright file="PartialExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IPartial"/>.
/// </summary>
public static class PartialExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IPartial
#pragma warning restore CA1034
    {
        /// <summary>
        /// <see langword="partial"/>Gets .
        /// </summary>
        public TItem Partial
        {
            get
            {
                instance.IsPartial = true;
                return instance;
            }
        }

        internal void AddPartial(ref SourceBuilder builder)
        {
            if (!instance.IsPartial)
            {
                return;
            }

            builder.Append("partial ");
        }
    }
}