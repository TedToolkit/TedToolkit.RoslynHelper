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
[Obsolete("Do not use this method, try to use the generators instead!")]
public interface ITypeParametersName
{
    /// <summary>
    ///     Gets a value indicating whether has the type parameters.
    /// </summary>
    bool HasTypeParameters { get; }

    /// <summary>
    ///     Gets the type parameters symbol.
    /// </summary>
    IReadOnlyList<TypeParamName> TypeParameters { get; }
}