// -----------------------------------------------------------------------
// <copyright file="Parameter.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The Parameter
/// </summary>
/// <param name="Type">The Parameter Type</param>
/// <param name="Identifier">The Variable</param>
public record struct Parameter(DataType Type, string Identifier) :
    IToCode,
    IDescription,
    IVariable,
    IAttributes,
    IStorageKind
{
    /// <inheritdoc />
    public List<IDescriptionItem> Descriptions
        => field ??= [];

    /// <inheritdoc />
    public IRootDescriptionItem ToRoot()
        => new DescriptionParam(Variable, Descriptions);

    /// <inheritdoc/>
    public readonly string Variable
        => Identifier.ToArgumentName();

    /// <summary>
    /// The default value.
    /// </summary>
    public IExpression? Default { get; internal set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddAttributes(ref builder);
        this.AddStorageKind(ref builder);
        Type.ToCode(ref builder);
        builder.Append(" @");
        builder.Append(Identifier);
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