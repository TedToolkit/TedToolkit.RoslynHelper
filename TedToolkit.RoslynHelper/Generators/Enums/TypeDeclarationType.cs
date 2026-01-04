// -----------------------------------------------------------------------
// <copyright file="TypeDeclarationType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Types;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The Type of the <see cref="TypeDeclaration"/>
/// </summary>
public enum TypeDeclarationType
{
    /// <summary>
    /// <see langword="class"/>
    /// </summary>
    CLASS = 0,

    /// <summary>
    /// <see langword="struct"/>
    /// </summary>
    STRUCT = 1,

    /// <summary>
    /// <see langword="ref"/> <see langword="struct"/>
    /// </summary>
    REF_STRUCT = 2,

    /// <summary>
    /// <see langword="record"/>
    /// </summary>
    RECORD = 3,

    /// <summary>
    /// <see langword="record"/> <see langword="struct"/>
    /// </summary>
    RECORD_STRUCT = 4,
}