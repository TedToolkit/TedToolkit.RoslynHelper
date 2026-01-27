// -----------------------------------------------------------------------
// <copyright file="IStorageKind.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The ref Type.
/// </summary>
public interface IStorageKind
{
    /// <summary>
    /// Gets or sets ref Type.
    /// </summary>
    StorageKind StorageKind { get; set; }
}