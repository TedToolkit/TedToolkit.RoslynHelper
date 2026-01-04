// -----------------------------------------------------------------------
// <copyright file="ReadonlyExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IReadonly"/>
/// </summary>
public static class ReadonlyExtensions
{
#pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IReadonly
#pragma warning restore CA1034
    {
        /// <summary>
        /// <see langword="partial"/>
        /// </summary>
        public ref TItem Readonly
        {
            get
            {
                instance.IsReadonly = true;
                return ref instance;
            }
        }

        internal void AddReadonly(ref SourceBuilder builder)
        {
            if (!instance.IsReadonly)
                return;

            builder.Append("readonly ");
        }
    }
}