// -----------------------------------------------------------------------
// <copyright file="AttributeModifier.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The type of the attribute
/// </summary>
public enum AttributeModifier
{
    /// <summary>
    /// None
    /// </summary>
    NONE = 0,

    /// <summary>
    /// Field
    /// </summary>
    FIELD = 1,

    /// <summary>
    /// Return
    /// </summary>
    RETURN = 2,

    /// <summary>
    /// Assembly
    /// </summary>
    ASSEMBLY = 3,

    /// <summary>
    /// Module
    /// </summary>
    MODULE = 4,

    /// <summary>
    /// Type
    /// </summary>
    TYPE = 5,

    /// <summary>
    /// Property
    /// </summary>
    PROPERTY = 6,

    /// <summary>
    /// Event
    /// </summary>
    EVENT = 7,

    /// <summary>
    /// Param
    /// </summary>
    PARAM = 8,
}