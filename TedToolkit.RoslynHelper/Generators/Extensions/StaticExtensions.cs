// -----------------------------------------------------------------------
// <copyright file="StaticExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IStatic"/>
/// </summary>
public static class StaticExtensions
{
    #pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IStatic
#pragma warning restore CA1034
    {
        /// <summary>
        /// <see langword="static"/>
        /// </summary>
        public ref TItem Static
        {
            get
            {
                instance.IsStatic = true;
                return ref instance;
            }
        }

        internal void AddStatic(ref SourceBuilder builder)
        {
            if (!instance.IsStatic)
                return;

            builder.Append("static ");
        }
    }
}