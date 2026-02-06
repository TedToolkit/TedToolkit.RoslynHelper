// -----------------------------------------------------------------------
// <copyright file="TypeParametersName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

using TedToolkit.RoslynHelper.Extensions;

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
///     For the one has type parameters.
/// </summary>
/// <typeparam name="T">The type.</typeparam>
[Obsolete("Do not use this method, try to use the generators instead!")]
public abstract class TypeParametersName<T> : BaseName<T>, ITypeParametersName
    where T : ISymbol
{
    private readonly Lazy<TypeParamName[]> _lazyTypeParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParametersName{T}"/> class.
    /// Create the type parameters.
    /// </summary>
    /// <param name="symbol">symbol.</param>
    private protected TypeParametersName(T symbol)
        : base(symbol)
    {
        _lazyTypeParameters = new(() => GetTypeParameters(symbol).GetNames().ToArray());
    }

    /// <inheritdoc />
    public bool HasTypeParameters
        => TypeParameters.Count > 0;

    /// <inheritdoc />
    public IReadOnlyList<TypeParamName> TypeParameters
        => _lazyTypeParameters.Value;

    /// <summary>
    /// Get the type parameters.
    /// </summary>
    /// <param name="symbol">Symbol.</param>
    /// <returns>Symbols.</returns>
    private protected abstract IEnumerable<ITypeParameterSymbol> GetTypeParameters(T symbol);
}