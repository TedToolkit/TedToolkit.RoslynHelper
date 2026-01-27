// -----------------------------------------------------------------------
// <copyright file="StorageKind.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The type of the parameter.
/// </summary>
public enum StorageKind
{
    /// <summary>
    /// No declarations.
    /// </summary>
    NONE = 0,

    /// <summary>
    /// <see langword="in"/>.
    /// </summary>
    IN = 1,

    /// <summary>
    /// <see langword="out"/>.
    /// </summary>
    OUT = 2,

    /// <summary>
    /// <see langword="ref"/>.
    /// </summary>
    REF = 3,

    /// <summary>
    /// <see langword="ref"/> <see langword="readonly"/>.
    /// </summary>
    REF_READONLY = 4,

    /// <summary>
    /// <see langword="scoped"/> <see langword="in"/>.
    /// </summary>
    SCOPED_IN = 5,

    /// <summary>
    /// <see langword="scoped"/> <see langword="ref"/>.
    /// </summary>
    SCOPED_REF = 6,

    /// <summary>
    /// <see langword="scoped"/> <see langword="ref"/> <see langword="readonly"/>.
    /// </summary>
    SCOPED_REF_READONLY = 7,
}