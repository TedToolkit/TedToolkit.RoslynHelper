// -----------------------------------------------------------------------
// <copyright file="PrefixUnaryExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Prefix Unary Expression.
/// </summary>
/// <param name="operator">operator.</param>
/// <param name="expression">expression.</param>
public sealed class PrefixUnaryExpression(string @operator, IExpression expression) :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append(@operator);
        builder.Append(' ');
        expression.ToCode(ref builder);
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        ToCode(ref builder);
    }
}