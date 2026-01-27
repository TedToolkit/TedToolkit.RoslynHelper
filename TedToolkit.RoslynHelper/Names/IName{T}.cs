// -----------------------------------------------------------------------
// <copyright file="IName{T}.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
/// The I Name.
/// </summary>
/// <typeparam name="T">The type of the symbol.</typeparam>
public interface IName<out T> : IName
    where T : ISymbol
{
    /// <summary>
    ///     Gets symbol.
    /// </summary>
    T Symbol { get; }
}