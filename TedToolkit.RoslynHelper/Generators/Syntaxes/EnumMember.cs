// -----------------------------------------------------------------------
// <copyright file="EnumMember.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The enum member.
/// </summary>
/// <param name="identifier">identifier.</param>
/// <param name="value">value.</param>
public sealed class EnumMember(string identifier, IExpression? value = null) :
    IRootDescription,
    IAttributes,
    IToCode
{
    /// <inheritdoc/>
    public List<IRootDescriptionItem> RootDescriptions
        => field ??= [];

    /// <inheritdoc/>
    public List<Attribute> Attributes
        => field ??= [];

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);
        this.AddAttributes(ref builder);
        builder.Append(identifier.ToValidIdentifier());

        if (value is not null)
        {
            builder.Append(" = ");
            value.ToCode(ref builder);
        }

        builder.Append(',');
    }
}