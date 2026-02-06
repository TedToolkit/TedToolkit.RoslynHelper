// -----------------------------------------------------------------------
// <copyright file="Delegate.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The delegate.
/// </summary>
/// <param name="identifier">the identifier.</param>
/// <param name="returnType">the return type.</param>
#pragma warning disable CA1711
public sealed class Delegate(string identifier, ReturnType? returnType = null) :
#pragma warning restore CA1711
    IMember,
    IParameters,
    IAttributes,
    IAccessibility,
    IUnsafe,
    IRootDescription
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);
        foreach (var parameter in Parameters)
        {
            parameter.ToRoot().ToDescription(ref builder);
        }

        returnType?.ToRoot().ToDescription(ref builder);

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddUnsafe(ref builder);

        builder.Append("delegate ");

        if (returnType is not null)
        {
            returnType.ToCode(ref builder);
        }
        else
        {
            builder.Append("void");
        }

        builder.Append(' ');

        builder.Append(identifier.ToValidIdentifier());
        this.AddParametersNoSkip(ref builder);
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
    public List<IRootDescriptionItem> RootDescriptions
        => field ??= [];
}