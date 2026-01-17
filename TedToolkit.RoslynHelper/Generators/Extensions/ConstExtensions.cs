// -----------------------------------------------------------------------
// <copyright file="ConstExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IConst"/>
/// </summary>
public static class ConstExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IConst
#pragma warning restore CA1034
    {
        /// <summary>
        /// <see langword="const"/>
        /// </summary>
        public TItem Const
        {
            get
            {
                instance.IsConst = true;
                return instance;
            }
        }

        internal void AddConst(ref SourceBuilder builder)
        {
            if (!instance.IsConst)
                return;

            builder.Append("const ");
        }
    }
}