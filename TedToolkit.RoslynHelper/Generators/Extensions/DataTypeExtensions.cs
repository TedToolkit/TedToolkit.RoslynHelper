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
        /// Make Array
        /// </summary>
        public ref DataType Array
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                type.IsArray = true;
                return ref type;
            }
        }

        /// <summary>
        /// Make Null
        /// </summary>
        public ref DataType Null
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                type.Type = type.Type.Null;
                return ref type;
            }
        }

        /// <summary>
        /// Pointer
        /// </summary>
#pragma warning disable CA1720
        public ref DataType Pointer
#pragma warning restore CA1720
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                type.PointCounter++;
                return ref type;
            }
        }

        /// <summary>
        /// Generic the items.
        /// </summary>
        /// <param name="types">types</param>
        /// <returns>expression</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DataType Generic(params DataType[] types)
        {
            type.Type = type.Type.Generic(types);
            return ref type;
        }
    }
}