// -----------------------------------------------------------------------
// <copyright file="Attribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Attribute
/// </summary>
/// <param name="Type">The Type</param>
#pragma warning disable CA1711
public record struct Attribute(IExpression Type) :
#pragma warning restore CA1711
    IToCode,
    IArguments
{
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

        Type.ToCode(ref builder);
        this.AddArguments(ref builder);
    }

    /// <summary>
    /// The modifier of the attribute.
    /// </summary>
    public AttributeModifier Modifier { get; set; }

    /// <inheritdoc />
    public List<Argument> Arguments
        => field ??= [];
}