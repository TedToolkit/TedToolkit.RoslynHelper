// -----------------------------------------------------------------------
// <copyright file="ITypeParameters.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// For the items that has type parameters.
/// </summary>
public interface ITypeParameters
{
    /// <summary>
    /// Gets the type parameters.
    /// </summary>
    List<TypeParameter> TypeParameters { get; }
}