// -----------------------------------------------------------------------
// <copyright file="IPartial.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// <see langword="partial"/>.
/// </summary>
public interface IPartial
{
    /// <summary>
    /// Gets or sets a value indicating whether <see langword="partial"/>.
    /// </summary>
    bool IsPartial { get; set; }
}