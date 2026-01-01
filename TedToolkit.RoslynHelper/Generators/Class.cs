// -----------------------------------------------------------------------
// <copyright file="Class.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The builder for class
/// </summary>
/// <param name="Identifier">identifier</param>
public record struct Class(string Identifier) :
    IAccessibility,
    IUnsafe,
    IStatic,
    IPartial,
    ICode,
    IAttributes
{
    /// <inheritdoc />
    public string ToCode()
    {
        var builder = ZString.CreateStringBuilder();
        try
        {
            this.AddAttributes(ref builder);
            this.AddAccessibility(ref builder);
            this.AddUnsafe(ref builder);
            this.AddStatic(ref builder);
            this.AddPartial(ref builder);
            builder.Append(Identifier);
            builder.AppendLine();
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <inheritdoc />
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc />
    public bool IsUnsafe { get; set; }

    /// <inheritdoc />
    public bool IsStatic { get; set; }

    /// <inheritdoc />
    public bool IsPartial { get; set; }

    /// <inheritdoc />
    public List<Attribute> Attributes
        => field ??= [];
}