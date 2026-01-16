// -----------------------------------------------------------------------
// <copyright file="ParameterKind.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The kind of the parameter.
/// </summary>
public enum ParameterKind
{
    /// <summary>
    /// No declarations.
    /// </summary>
    NONE = 0,

    /// <summary>
    /// <see langword="this"/>
    /// </summary>
    THIS = 1,

    /// <summary>
    /// <see langword="params"/>
    /// </summary>
    PARAMS = 2,
}