// -----------------------------------------------------------------------
// <copyright file="Accessor.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The accessor
/// </summary>
/// <param name="Type">Type of the accessor</param>
public record struct Accessor(AccessorType Type) :
    IToCode,
    IAttributes,
    IAccessibility,
    IUnsafe,
    IReadonly,
    IStatementOwner
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddReadonly(ref builder);
        this.AddUnsafe(ref builder);
        builder.Append(Type switch
        {
            AccessorType.GET => "get",
            AccessorType.SET => "set",
            AccessorType.INIT => "init",
            AccessorType.ADD => "add",
            AccessorType.REMOVE => "remove",
            _ => throw new InvalidOperationException(),
        });
        this.AddStatements(ref builder);
    }

    /// <inheritdoc/>
    public List<Attribute> Attributes
        => field ??= [];

    /// <inheritdoc/>
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc/>
    public bool IsUnsafe { get; set; }

    /// <inheritdoc/>
    public List<IStatement> Statements
        => field ??= [];

    /// <inheritdoc />
    public bool IsReadonly { get; set; }
}