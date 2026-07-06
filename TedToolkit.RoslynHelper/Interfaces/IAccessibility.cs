// -----------------------------------------------------------------------
// <copyright file="IAccessibility.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The Item that has Accessibility.
/// </summary>
public interface IAccessibility
{
    /// <summary>
    /// Gets or sets the Accessibility.
    /// </summary>
    Accessibility Accessibility { get; set; }
}