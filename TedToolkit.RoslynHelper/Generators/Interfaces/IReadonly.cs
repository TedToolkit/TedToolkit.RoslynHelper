// -----------------------------------------------------------------------
// <copyright file="IReadonly.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// <see langword="readonly"/>.
/// </summary>
public interface IReadonly
{
    /// <summary>
    /// Gets or sets a value indicating whether <see langword="readonly"/>.
    /// </summary>
    bool IsReadonly { get; set; }
}