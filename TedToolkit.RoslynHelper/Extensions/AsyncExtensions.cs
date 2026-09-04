// -----------------------------------------------------------------------
// <copyright file="AsyncExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// Extensions for declarations supporting <see langword="async"/>.
/// </summary>
public static class AsyncExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IAsync
#pragma warning restore CA1034
    {
        /// <summary>
        /// Gets the declaration with the <see langword="async"/> modifier, preserving its return type.
        /// </summary>
        public TItem Async
        {
            get
            {
                instance.IsAsync = true;
                return instance;
            }
        }

        internal void AddAsyncModifier(ref SourceBuilder builder)
        {
            if (!instance.IsAsync)
            {
                return;
            }

            builder.Append("async ");
        }
    }
}