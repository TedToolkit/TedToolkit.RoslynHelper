// -----------------------------------------------------------------------
// <copyright file="PostfixUnaryExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Postfix unary expression.
/// </summary>
/// <param name="expression">expression.</param>
/// <param name="operator">operator.</param>
public sealed class PostfixUnaryExpression(IExpression expression, string @operator) :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        expression.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(@operator);
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
        => ToCode(ref builder);
}