// -----------------------------------------------------------------------
// <copyright file="UnsafeExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IUnsafe"/>
/// </summary>
public static class UnsafeExtensions
{
    #pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IUnsafe
#pragma warning restore CA1034
    {
        /// <summary>
        /// <see langword="unsafe"/>
        /// </summary>
        public ref TItem Unsafe
        {
            get
            {
                instance.IsUnsafe = true;
                return ref instance;
            }
        }

        internal void AddUnsafe(ref Utf16ValueStringBuilder builder)
        {
            if (!instance.IsUnsafe)
                return;

            builder.Append("unsafe ");
        }
    }
}