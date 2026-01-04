// -----------------------------------------------------------------------
// <copyright file="Method.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The method
/// </summary>
/// <param name="Identifier">name</param>
/// <param name="ReturnType">ReturnType</param>
public record struct Method(string Identifier, ReturnType? ReturnType = null) :
    IMember,
    IParameters,
    IAttributes,
    IAccessibility,
    IUnsafe,
    IPartial,
    IStatic,
    IPolymorphism,
    IDescription
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddSummary(ref builder);
        this.AddParametersSummary(ref builder);
        if (ReturnType is { Description.Count: > 0, } returnType)
        {
            builder.AppendLine("/// <result>");
            returnType.AddDescriptionItems(ref builder);
            builder.AppendLine("/// </result>");
        }

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddUnsafe(ref builder);
        this.AddPartial(ref builder);

        if (ReturnType.HasValue)
            ReturnType.Value.Type.ToCode(ref builder);
        else
            builder.Append("void");

        builder.Append(" ");

        builder.Append(Identifier);
        this.AddParameters(ref builder);
    }

    /// <inheritdoc/>
    public List<Parameter> Parameters
        => field ??= [];

    /// <inheritdoc/>
    public List<Attribute> Attributes
        => field ??= [];

    /// <inheritdoc/>
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc/>
    public bool IsUnsafe { get; set; }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc />
    public bool IsStatic { get; set; }

    /// <inheritdoc />
    public Polymorphism Polymorphism { get; set; }

    /// <inheritdoc />
    public List<string> Description
        => field ??= [];
}