// -----------------------------------------------------------------------
// <copyright file="ITypeParametersName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
/// The Type parameter name.
/// </summary>
public interface ITypeParametersName
{
    /// <summary>
    ///     Gets a value indicating whether has the type parameters.
    /// </summary>
    bool HasTypeParameters { get; }

    /// <summary>
    ///     Gets get the type parameters symbol.
    /// </summary>
    IReadOnlyList<TypeParamName> TypeParameters { get; }
}