// -----------------------------------------------------------------------
// <copyright file="Attribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Attribute.
/// </summary>
/// <param name="type">The Type.</param>
#pragma warning disable CA1711
public sealed class Attribute(DataType type) :
#pragma warning restore CA1711
    IToCode,
    IArguments
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Attribute"/> class.
    /// Create from a symbol.
    /// </summary>
    /// <param name="type">symbol.</param>
    /// <param name="compilation">compilation.</param>
    public Attribute(ITypeSymbol type, Compilation? compilation = null)
        : this(DataType.FromSymbol(type, compilation))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Attribute"/> class.
    /// Create from a type.
    /// </summary>
    /// <param name="type">type.</param>
    public Attribute(Type type)
        : this(DataType.FromType(type))
    {
    }

    /// <summary>
    /// Add a named argument to the attribute.
    /// </summary>
    /// <param name="name">name.</param>
    /// <param name="argument">argument.</param>
    /// <returns>self.</returns>
    /// <exception cref="ArgumentNullException">name is null.</exception>
    public Attribute AddNamedArgument(string name, IExpression argument)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        Arguments.Add(new Argument(name.ToSimpleName().Operator("=", argument)));
        return this;
    }

    /// <summary>
    /// Create from a symbol.
    /// </summary>
    /// <param name="attribute">attribute data.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException">attribute is null.</exception>
    public static Attribute FromSymbol(AttributeData attribute, Compilation? compilation = null)
    {
        if (attribute?.AttributeClass is null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        var result = new Attribute(DataType.FromSymbol(attribute.AttributeClass, compilation));

        foreach (var attributeConstructorArgument in attribute.ConstructorArguments)
        {
            if (GetArgument(attributeConstructorArgument, compilation) is { } argument)
            {
                result.AddArgument(new Argument(argument));
            }
        }

        foreach (var attributeNamedArgument in attribute.NamedArguments)
        {
            if (GetArgument(attributeNamedArgument.Value, compilation) is { } argument)
            {
                result.AddArgument(new Argument(attributeNamedArgument.Key.ToSimpleName()
                    .Operator("=", argument)));
            }
        }

        return result;
    }

    private static IExpression? GetArgument(scoped in TypedConstant argument, Compilation? compilation = null)
    {
        switch (argument.Kind)
        {
            case TypedConstantKind.Error:
                return SimpleNameExpression.Null;

            case TypedConstantKind.Primitive:
                if (argument.Value is string str)
                {
                    return str.ToLiteral();
                }

                return argument.Value?.ToString().ToSimpleName();

            case TypedConstantKind.Enum:
                if (argument.Type is not { } symbol)
                {
                    return null;
                }

                return argument.Value!.ToString().ToSimpleName().Cast(DataType.FromSymbol(symbol, compilation));

            case TypedConstantKind.Type:
                if (argument.Value is not Type type)
                {
                    return null;
                }

                return "typeof".ToSimpleName().Invoke().AddArgument(
                    new Argument(DataType.FromType(type).Type));

            case TypedConstantKind.Array:
                var collection = new CollectionExpression();
                foreach (var argumentValue in argument.Values)
                {
                    if (GetArgument(argumentValue, compilation) is { } item)
                    {
                        collection.AddElement(new CollectionElement(item));
                    }
                }

                return collection;

            default:
                return null;
        }
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
    /// Gets or sets the modifier of the attribute.
    /// </summary>
    public AttributeModifier Modifier { get; set; }

    /// <summary>
    /// Add modifier.
    /// </summary>
    /// <param name="modifier">modifier.</param>
    /// <returns>the item.</returns>
    public Attribute AddModifier(AttributeModifier modifier)
    {
        Modifier = modifier;
        return this;
    }

    /// <inheritdoc />
    public List<Argument> Arguments
    {
        get
        {
            return field ??= [];
        }
    }
}