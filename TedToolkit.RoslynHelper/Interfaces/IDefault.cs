// -----------------------------------------------------------------------
// <copyright file="IDefault.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// Get the default value.
/// </summary>
public interface IDefault
{
    /// <summary>
    /// Gets or sets the default value.
    /// </summary>
    IExpression? Default { get; set; }
}