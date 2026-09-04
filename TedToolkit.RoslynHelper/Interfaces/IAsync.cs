// -----------------------------------------------------------------------
// <copyright file="IAsync.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// A declaration that supports the <see langword="async"/> modifier.
/// </summary>
public interface IAsync
{
    /// <summary>
    /// Gets or sets a value indicating whether the declaration is asynchronous.
    /// </summary>
    bool IsAsync { get; set; }
}