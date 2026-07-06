// -----------------------------------------------------------------------
// <copyright file="Polymorphism.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The type of the polymorphism.
/// </summary>
public enum Polymorphism
{
    /// <summary>
    /// None.
    /// </summary>
    NONE = 0,

    /// <summary>
    /// <see langword="virtual"/>.
    /// </summary>
    VIRTUAL = 1,

    /// <summary>
    /// <see langword="abstract"/>.
    /// </summary>
    ABSTRACT = 2,

    /// <summary>
    /// <see langword="override"/>.
    /// </summary>
    OVERRIDE = 3,

    /// <summary>
    /// <see langword="sealed"/>.
    /// </summary>
    SEALED = 4,

    /// <summary>
    /// <see langword="sealed"/> <see langword="override"/>.
    /// </summary>
    SEALED_OVERRIDE = 5,

    /// <summary>
    /// <see langword="new"/>.
    /// </summary>
    NEW = 6,
}