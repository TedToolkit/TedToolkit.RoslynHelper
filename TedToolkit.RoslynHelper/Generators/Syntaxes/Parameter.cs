// -----------------------------------------------------------------------
// <copyright file="Parameter.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The Parameter
/// </summary>
/// <param name="type">The Parameter Type</param>
/// <param name="identifier">The Variable</param>
public sealed class Parameter(DataType type, string identifier) :
    IToCode,
    IDescription,
    IVariable,
    IAttributes,
    IStorageKind
{
    /// <summary>
    /// Create from a type
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    public Parameter(Type type, string identifier)
        : this(DataType.FromType(type), identifier)
    {
    }

    /// <summary>
    /// Create from a type
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    public Parameter(ITypeSymbol type, string identifier)
        : this(new DataType(type), identifier)
    {
    }

    /// <inheritdoc />
    public List<IDescriptionItem> Descriptions
        => field ??= [];

    /// <inheritdoc />
    public IRootDescriptionItem ToRoot()
        => new DescriptionParam(Variable, Descriptions);

    /// <inheritdoc/>
    public string Variable
        => identifier.ToArgumentName();

    /// <summary>
    /// The default value.
    /// </summary>
    public IExpression? Default { get; internal set; }

    /// <summary>
    /// Add null
    /// </summary>
    /// <returns>self</returns>
    public Parameter AddNull()
    {
        Default = SimpleNameExpression.Null;
        return this;
    }

    /// <summary>
    /// Add default
    /// </summary>
    /// <returns>self</returns>
    public Parameter AddDefault()
    {
        Default = SimpleNameExpression.Default;
        return this;
    }

    /// <summary>
    /// Add default
    /// </summary>
    /// <param name="value">defaultValue</param>
    /// <returns>self</returns>
    public Parameter AddDefault(IExpression value)
    {
        Default = value;
        return this;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddAttributes(ref builder);
        this.AddStorageKind(ref builder);
        type.ToCode(ref builder);
        builder.Append(" @");
        builder.Append(identifier);
        if (Default is null)
            return;

        builder.Append(" = ");
        Default.ToCode(ref builder);
    }

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <inheritdoc />
    public List<Attribute> Attributes
        => field ??= [];
}