// -----------------------------------------------------------------------
// <copyright file="CollectionExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The collection expression
/// </summary>
public sealed class CollectionExpression :
    IExpression,
    IExpressionOwner
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append('[');
        builder.Indent();

        foreach (var expression in Expressions)
        {
            builder.AppendLine();
            expression.ToCode(ref builder);
            builder.Append(',');
        }

        builder.Dedent();
        builder.AppendLine();
        builder.Append(']');
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
    }

    /// <inheritdoc />
    public List<IExpression> Expressions
        => field ??= [];
}