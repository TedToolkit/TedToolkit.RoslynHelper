// -----------------------------------------------------------------------
// <copyright file="VariableExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IVariable"/>
/// </summary>
public static class VariableExtensions
{
#pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IVariable
#pragma warning restore CA1034
    {
        /// <summary>
        /// Get the name
        /// </summary>
        public SimpleNameExpression Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(instance.Variable);
        }
    }
}