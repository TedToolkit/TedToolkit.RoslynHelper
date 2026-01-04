// -----------------------------------------------------------------------
// <copyright file="IVariables.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// For the items that has Variable for calling.
/// </summary>
public interface IVariables
{
    /// <summary>
    /// The identifier
    /// </summary>
    string Variable { get; }
}