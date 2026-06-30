// -----------------------------------------------------------------------
// <copyright file="IAccessors.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// Get the accessors.
/// </summary>
public interface IAccessors
{
    /// <summary>
    /// Gets accessors.
    /// </summary>
    List<Accessor> Accessors { get; }
}