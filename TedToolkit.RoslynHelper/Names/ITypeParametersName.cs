// -----------------------------------------------------------------------
// <copyright file="ITypeParametersName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
/// </summary>
public interface ITypeParametersName
{
    /// <summary>
    ///     Has the type parameters.
    /// </summary>
    bool HasTypeParameters { get; }

    /// <summary>
    ///     Get the type parameters symbol
    /// </summary>
    TypeParamName[] TypeParameters { get; }
}