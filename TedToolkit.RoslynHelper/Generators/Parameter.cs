// -----------------------------------------------------------------------
// <copyright file="Parameter.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The Parameter
/// </summary>
/// <param name="Type">The Parameter Type</param>
/// <param name="Identifier">The Identifier</param>
public record struct Parameter(MemberAccess Type, string Identifier) :
    IToCode,
    IToDescription,
    IDescription
{
    /// <inheritdoc />
    public Description Description { get; }

    /// <summary>
    /// The default value.
    /// </summary>
    public string Default { get; internal set; } = "";

    /// <inheritdoc />
    public readonly string ToCode()
    {
        using var builder = ZString.CreateStringBuilder();

        builder.Append(Type.ToString());
        builder.Append(' ');
        builder.Append(Identifier);
        if (!string.IsNullOrEmpty(Default))
        {
            builder.Append(" = ");
            builder.Append(Default);
        }

        return builder.ToString();
    }

    /// <inheritdoc />
    public readonly string ToDescription()
    {
        using var builder = ZString.CreateStringBuilder();
        builder.Append("<param name=\"");
        builder.Append(Identifier);
        builder.Append("\">");
        builder.Append(Description.ToDescription());
        builder.Append("</param>");
        return builder.ToString();
    }
}