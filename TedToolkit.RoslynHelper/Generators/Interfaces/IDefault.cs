// -----------------------------------------------------------------------
// <copyright file="IDefault.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// Get the default value.
/// </summary>
public interface IDefault
{
    /// <summary>
    /// The default value.
    /// </summary>
    IExpression? Default { get; set; }
}