// -----------------------------------------------------------------------
// <copyright file="Delegate.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The delegate
/// </summary>
/// <param name="Identifier">the identifier</param>
/// <param name="ReturnType">the return type</param>
#pragma warning disable CA1711
public record struct Delegate(string Identifier, ReturnType? ReturnType = null) :
#pragma warning restore CA1711
    IMember,
    IParameters,
    IAttributes,
    IAccessibility,
    IUnsafe,
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
        this.AddUnsafe(ref builder);

        builder.Append("delegate ");

        if (ReturnType.HasValue)
            ReturnType.Value.ToCode(ref builder);
        else
            builder.Append("void");

        builder.Append(' ');

        builder.Append(Identifier);
        this.AddParametersNoReturn(ref builder);
        builder.Append(';');
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
    public List<string> Description
        => field ??= [];
}