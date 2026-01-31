// -----------------------------------------------------------------------
// <copyright file="ReturnType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The Return Type.
/// </summary>
/// <param name="type">return Type.</param>
public sealed class ReturnType(DataType type) :
    IDescription,
    IToCode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReturnType"/> class.
    /// Create from a symbol.
    /// </summary>
    /// <param name="type">symbol.</param>
    public ReturnType(ITypeSymbol type)
        : this(DataType.FromSymbol(type))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReturnType"/> class.
    /// Create from a type.
    /// </summary>
    /// <param name="type">type.</param>
    public ReturnType(Type type)
        : this(DataType.FromType(type))
    {
    }

    /// <inheritdoc />
    public List<IDescriptionItem> Descriptions
        => field ??= [];

    /// <inheritdoc />
    public IRootDescriptionItem ToRoot()
        => new DescriptionReturns(Descriptions);

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
        => type.ToCode(ref builder);
}