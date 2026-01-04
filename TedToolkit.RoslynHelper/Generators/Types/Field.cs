// -----------------------------------------------------------------------
// <copyright file="Field.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Types;

/// <summary>
/// The field
/// </summary>
/// <param name="Type">Field Type</param>
/// <param name="Identifier">IDentifier</param>
public record struct Field(MemberAccess Type, string Identifier) :
    IMember,
    IAttributes,
    IStatic,
    IAccessibility,
    IReadonly,
    IVariables,
    IDescription
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddSummary(ref builder);

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddReadonly(ref builder);

        Type.ToCode(ref builder);
        builder.Append(' ');

        builder.Append(Identifier);
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
    public List<string> Description
        => field ??= [];

    /// <inheritdoc />
    public string Variable
    {
        readonly get => Identifier;

        set => Identifier = value;
    }
}