// -----------------------------------------------------------------------
// <copyright file="DataTypeExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="DataType"/>
/// </summary>
public static class DataTypeExtensions
{
#pragma warning disable CA1034
    extension(ref DataType type)
#pragma warning restore CA1034
    {
        /// <summary>
        /// Make Null
        /// </summary>
        public ref DataType Array
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                type.Type = type.Type.Array;
                return ref type;
            }
        }
    }
}