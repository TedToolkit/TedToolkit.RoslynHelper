// -----------------------------------------------------------------------
// <copyright file="AwaitExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// Extensions for statements supporting asynchronous iteration or disposal.
/// </summary>
public static class AwaitExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IAwait
#pragma warning restore CA1034
    {
        /// <summary>
        /// Gets the statement with the <see langword="await"/> modifier.
        /// </summary>
        public TItem Await
        {
            get
            {
                instance.IsAwait = true;
                return instance;
            }
        }

        internal void AddAwait(ref SourceBuilder builder)
        {
            if (!instance.IsAwait)
            {
                return;
            }

            builder.Append("await ");
        }
    }
}