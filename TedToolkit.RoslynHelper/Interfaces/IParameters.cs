// -----------------------------------------------------------------------
// <copyright file="IParameters.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// For the items that has Parameters.
/// </summary>
public interface IParameters
{
    /// <summary>
    /// Gets the parameters.
    /// </summary>
    List<Parameter> Parameters { get; }
}