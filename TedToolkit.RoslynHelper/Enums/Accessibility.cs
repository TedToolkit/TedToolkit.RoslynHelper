// -----------------------------------------------------------------------
// <copyright file="Accessibility.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The accessibility for the objects.
/// </summary>
public enum Accessibility
{
    /// <summary>
    /// Nothing about it.
    /// </summary>
    NONE = 0,

    /// <summary>
    /// <see langword="public"/>.
    /// </summary>
    PUBLIC = 1,

    /// <summary>
    /// <see langword="internal"/>.
    /// </summary>
    INTERNAL = 2,

    /// <summary>
    /// <see langword="private"/>.
    /// </summary>
    PRIVATE = 3,

    /// <summary>
    /// <see langword="file"/>.
    /// </summary>
    FILE = 4,

    /// <summary>
    /// <see langword="private"/> <see langword="protected"/>.
    /// </summary>
    PRIVATE_PROTECTED = 5,

    /// <summary>
    /// <see langword="protected"/>.
    /// </summary>
    PROTECTED = 6,

    /// <summary>
    /// <see langword="protected"/> <see langword="internal"/>.
    /// </summary>
    PROTECTED_INTERNAL = 7,
}