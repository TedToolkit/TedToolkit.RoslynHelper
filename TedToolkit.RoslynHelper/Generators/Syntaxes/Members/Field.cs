// -----------------------------------------------------------------------
// <copyright file="Field.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The field.
/// </summary>
/// <param name="type">Field Type.</param>
/// <param name="identifier">Identifier.</param>
public sealed class Field(DataType type, string identifier) :
    IMember,
    IAttributes,
    IStatic,
    IAccessibility,
    IReadonly,
    IVariable,
    IRootDescription,
    IDefault,
    IConst
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddReadonly(ref builder);
        this.AddConst(ref builder);

        type.ToCode(ref builder);
        builder.Append(' ');

        builder.Append(identifier.ToValidIdentifier());
        this.AddDefault(ref builder);
        builder.Append(';');
    }

    /// <inheritdoc/>
    public List<Attribute> Attributes
        => field ??= [];

    /// <inheritdoc/>
    public bool IsStatic { get; set; }

    /// <inheritdoc/>
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc/>
    public bool IsReadonly { get; set; }

    /// <inheritdoc/>
    public List<IRootDescriptionItem> RootDescriptions
        => field ??= [];

    /// <inheritdoc/>
    public string Variable
        => identifier.ToValidIdentifier();

    /// <inheritdoc />
    public IExpression? Default { get; set; }

    /// <inheritdoc />
    public bool IsConst { get; set; }
}