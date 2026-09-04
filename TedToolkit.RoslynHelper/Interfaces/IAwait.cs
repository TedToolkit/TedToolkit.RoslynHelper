// -----------------------------------------------------------------------
// <copyright file="IAwait.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// A statement that supports an <see langword="await"/> modifier.
/// </summary>
public interface IAwait
{
    /// <summary>
    /// Gets or sets a value indicating whether the statement uses asynchronous iteration or disposal.
    /// </summary>
    bool IsAwait { get; set; }
}