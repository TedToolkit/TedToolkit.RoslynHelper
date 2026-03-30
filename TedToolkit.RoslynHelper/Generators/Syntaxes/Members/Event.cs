// -----------------------------------------------------------------------
// <copyright file="Event.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Create an event.
/// </summary>
/// <param name="type">event type.</param>
/// <param name="identifier">identifier.</param>
public sealed class Event(DataType type, string identifier) :
    IMember,
    IVariable,
    IAccessibility,
    IPartial,
    IStatic,
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
        this.AddStatic(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddPartial(ref builder);

        builder.Append("event ");
        type.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(identifier.ToValidIdentifier());
        this.AddAccessors(ref builder);
    }

    /// <inheritdoc/>
    public string Variable
    {
        get
        {
            return identifier.ToValidIdentifier();
        }
    }

    /// <inheritdoc/>
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc/>
    public bool IsStatic { get; set; }

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
}