// -----------------------------------------------------------------------
// <copyright file="IAttributes.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// HasAttributes.
/// </summary>
public interface IAttributes
{
    /// <summary>
    /// Gets attributes.
    /// </summary>
    List<Syntaxes.ConditionalItem<Syntaxes.Attribute>> Attributes { get; }
}
