// -----------------------------------------------------------------------
// <copyright file="Indexer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The indexer.
/// </summary>
/// <param name="type">data type.</param>
public sealed class Indexer(DataType type) :
    IMember,
    IAccessibility,
    IPartial,
    IParameters,
    IReadonly,
    IPolymorphism,
    IRootDescription,
    IAttributes,
    IAccessors
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddReadonly(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddPartial(ref builder);

        type.ToCode(ref builder);
        builder.Append(" this[");
        this.AddParametersList(ref builder);
        builder.Append("]");
        this.AddAccessors(ref builder);
    }

    /// <inheritdoc/>
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc/>
    public bool IsReadonly { get; set; }

    /// <inheritdoc/>
    public Polymorphism Polymorphism { get; set; }

    /// <inheritdoc/>
    public List<IRootDescriptionItem> RootDescriptions
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public List<Attribute> Attributes
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public List<Accessor> Accessors
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public List<Parameter> Parameters
    {
        get
        {
            return field ??= [];
        }
    }
}