// -----------------------------------------------------------------------
// <copyright file="Property.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Types;

/// <summary>
/// The property
/// </summary>
/// <param name="Type">The type</param>
/// <param name="Identifier">The identifier</param>
public record struct Property(MemberAccess Type, string Identifier) :
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
    /// <inheritdoc />
    public string Variable
    {
        readonly get => Identifier;

        set => Identifier = value;
    }

    /// <inheritdoc />
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddSummary(ref builder);

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddPartial(ref builder);

        Type.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(Identifier);

        builder.BeginBlock();
        foreach (var accessor in Accessors)
        {
            builder.AppendLine();
            accessor.ToCode(ref builder);
        }

        builder.EndBlock();
    }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc/>
    public bool IsStatic { get; set; }

    /// <inheritdoc/>
    public Polymorphism Polymorphism { get; set; }

    /// <inheritdoc/>
    public List<string> Description
        => field ??= [];

    /// <inheritdoc />
    public List<Attribute> Attributes
        => field ??= [];

    /// <inheritdoc />
    public List<Accessor> Accessors
        => field ??= [];
}