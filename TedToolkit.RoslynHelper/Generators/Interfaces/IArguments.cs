// -----------------------------------------------------------------------
// <copyright file="IArguments.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The arguments
/// </summary>
public interface IArguments
{
    /// <summary>
    /// Arguments
    /// </summary>
    List<Argument> Arguments { get; }
}