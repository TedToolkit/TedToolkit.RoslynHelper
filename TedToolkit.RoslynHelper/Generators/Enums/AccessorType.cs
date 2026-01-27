// -----------------------------------------------------------------------
// <copyright file="AccessorType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The type of the accessor.
/// </summary>
public enum AccessorType
{
    /// <summary>
    /// get.
    /// </summary>
    GET = 0,

    /// <summary>
    /// set.
    /// </summary>
    SET = 1,

    /// <summary>
    /// init.
    /// </summary>
    INIT = 2,

    /// <summary>
    /// add.
    /// </summary>
    ADD = 3,

    /// <summary>
    /// remove.
    /// </summary>
    REMOVE = 4,
}