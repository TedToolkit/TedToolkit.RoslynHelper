// -----------------------------------------------------------------------
// <copyright file="IStatic.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// <see langword="static"/>.
/// </summary>
public interface IStatic
{
    /// <summary>
    /// Gets or sets a value indicating whether <see langword="static"/>.
    /// </summary>
    bool IsStatic { get; set; }
}