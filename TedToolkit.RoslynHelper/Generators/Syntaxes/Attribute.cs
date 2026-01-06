// -----------------------------------------------------------------------
// <copyright file="Attribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Attribute
/// </summary>
/// <param name="type">The Type</param>
#pragma warning disable CA1711
public sealed class Attribute(DataType type) :
#pragma warning restore CA1711
    IToCode,
    IArguments
{
    /// <summary>
    /// Create from a symbol
    /// </summary>
    /// <param name="type">symbol</param>
    public Attribute(ITypeSymbol type)
        : this(new DataType(type))
    {
    }

    /// <summary>
    /// Create from a type
    /// </summary>
    /// <param name="type">type</param>
    public Attribute(Type type)
        : this(DataType.FromType(type))
    {
    }

    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append(Modifier switch
        {
            AttributeModifier.NONE => "",
            AttributeModifier.FIELD => "field:",
            AttributeModifier.RETURN => "return:",
            AttributeModifier.ASSEMBLY => "assembly:",
            AttributeModifier.MODULE => "module:",
            AttributeModifier.TYPE => "type:",
            AttributeModifier.PROPERTY => "property:",
            AttributeModifier.EVENT => "event:",
            AttributeModifier.PARAM => "param:",
            _ => throw new InvalidOperationException(nameof(Modifier)),
        });

        type.ToCode(ref builder);
        this.AddArguments(ref builder);
    }

    /// <summary>
    /// The modifier of the attribute.
    /// </summary>
    public AttributeModifier Modifier { get; set; }

    /// <summary>
    /// Add modifier
    /// </summary>
    /// <param name="modifier">modifier</param>
    /// <returns>the item</returns>
    public Attribute AddModifier(AttributeModifier modifier)
    {
        Modifier = modifier;
        return this;
    }

    /// <inheritdoc />
    public List<Argument> Arguments
        => field ??= [];
}