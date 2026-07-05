// -----------------------------------------------------------------------
// <copyright file="Conversion.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The Conversion things.
/// </summary>
/// <param name="type">Type.</param>
/// <param name="isFrom">Is from.</param>
/// <param name="isImplicit">is implicit.</param>
public sealed class Conversion(DataType type, bool isFrom, bool isImplicit) :
    ConditionalCompilationSyntax,
    IMember,
    IStatementOwner,
    IRootDescription,
    IStorageKind,
    IAttributes,
    IOwner
{
    /// <inheritdoc />
    public string Owner { get; set; } = "";

    /// <inheritdoc />
    protected override void WriteSyntax(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);
        this.AddAttributes(ref builder);

        builder.Append("public static ");

        builder.Append(isImplicit ? "implicit" : "explicit");

        builder.Append(" operator ");

        if (isFrom)
        {
            builder.Append(Owner);
        }
        else
        {
            type.ToCode(ref builder);
        }

        builder.Append('(');

        this.AddStorageKind(ref builder);

        if (isFrom)
        {
            type.ToCode(ref builder);
        }
        else
        {
            builder.Append(Owner);
        }

        builder.Append(" value)");

        this.AddStatements(ref builder);
    }

    /// <inheritdoc/>
    public List<IRootDescriptionItem> RootDescriptions
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public List<IStatement> Statements
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public List<ConditionalItem<Attribute>> Attributes
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public StorageKind StorageKind { get; set; }
}
