// -----------------------------------------------------------------------
// <copyright file="Event.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Types;

/// <summary>
/// Create an event
/// </summary>
/// <param name="Type">event type</param>
/// <param name="Identifier">identifier</param>
public record struct Event(MemberAccess Type, string Identifier) :
    IMember,
    IVariables,
    IAccessibility,
    IPartial,
    IStatic,
    IPolymorphism,
    IDescription,
    IAttributes,
    IAccessors
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddSummary(ref builder);

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddPartial(ref builder);

        builder.Append("event ");
        Type.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(Identifier);
        this.AddAccessors(ref builder);
    }

    /// <inheritdoc/>
    public readonly string Variable
        => Identifier;

    /// <inheritdoc/>
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc/>
    public bool IsStatic { get; set; }

    /// <inheritdoc/>
    public Polymorphism Polymorphism { get; set; }

    /// <inheritdoc/>
    public List<string> Description
        => field ??= [];

    /// <inheritdoc/>
    public List<Attribute> Attributes
        => field ??= [];

    /// <inheritdoc/>
    public List<Accessor> Accessors
        => field ??= [];
}