// -----------------------------------------------------------------------
// <copyright file="Property.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The property.
/// </summary>
/// <param name="type">The type.</param>
/// <param name="identifier">The identifier.</param>
public sealed class Property(DataType type, string identifier) :
    IMember,
    IVariable,
    IAccessibility,
    IPartial,
    IStatic,
    IReadonly,
    IPolymorphism,
    IRootDescription,
    IAttributes,
    IDefault,
    IAccessors
{
    /// <inheritdoc/>
    public string Variable
        => identifier.ToValidIdentifier();

    /// <inheritdoc />
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddReadonly(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddPartial(ref builder);

        type.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(identifier.ToValidIdentifier());
        this.AddAccessors(ref builder);

        if (Default is null)
        {
            return;
        }

        this.AddDefault(ref builder);
        builder.Append(';');
    }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc/>
    public bool IsStatic { get; set; }

    /// <inheritdoc/>
    public Polymorphism Polymorphism { get; set; }

    /// <inheritdoc/>
    public List<IRootDescriptionItem> RootDescriptions
        => field ??= [];

    /// <inheritdoc />
    public List<Attribute> Attributes
        => field ??= [];

    /// <inheritdoc />
    public List<Accessor> Accessors
        => field ??= [];

    /// <inheritdoc />
    public bool IsReadonly { get; set; }

    /// <inheritdoc />
    public IExpression? Default { get; set; }
}