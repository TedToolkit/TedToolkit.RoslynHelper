// -----------------------------------------------------------------------
// <copyright file="IUnsafe.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// <see langword="unsafe"/>.
/// </summary>
public interface IUnsafe
{
    /// <summary>
    /// Gets or sets a value indicating whether <see langword="unsafe"/>.
    /// </summary>
    bool IsUnsafe { get; set; }
}