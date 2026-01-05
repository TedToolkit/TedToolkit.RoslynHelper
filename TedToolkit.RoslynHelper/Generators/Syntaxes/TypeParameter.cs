// -----------------------------------------------------------------------
// <copyright file="TypeParameter.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The TypeParameters
/// </summary>
/// <param name="Identifier">identifier</param>
public record struct TypeParameter(string Identifier) :
    IToCode,
    IDescription,
    IVariable,
    IAttributes,
    IStorageKind
{
    /// <inheritdoc />
    public List<string> Description
        => field ??= [];

    /// <inheritdoc/>
    public readonly string Variable
        => Identifier;

    /// <inheritdoc />
    public List<Attribute> Attributes
        => field ??= [];

    /// <summary>
    /// Constraints
    /// </summary>
#pragma warning disable S2325
    public List<IExpression> Constraints
#pragma warning restore S2325
        => field ??= [];

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddAttributes(ref builder);
        this.AddStorageKind(ref builder);
        builder.Append(Identifier);
    }

    /// <summary>
    /// To the constraint
    /// </summary>
    /// <param name="builder">the builder</param>
    internal void ToConstraint(ref SourceBuilder builder)
    {
        if (Constraints.Count is 0)
            return;

        builder.AppendLine();
        builder.Append("\twhere ");
        builder.Append(Identifier);
        builder.Append(": ");

        var isNotStart = false;
        foreach (var constraint in Constraints)
        {
            if (isNotStart)
                builder.Append(", ");

            constraint.ToCode(ref builder);

            isNotStart = true;
        }
    }
}