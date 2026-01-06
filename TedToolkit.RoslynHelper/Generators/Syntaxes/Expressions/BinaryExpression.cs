// -----------------------------------------------------------------------
// <copyright file="BinaryExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Binary Expression
/// </summary>
/// <param name="operator">operator</param>
/// <param name="left">left</param>
/// <param name="right">right</param>
public sealed class BinaryExpression(string @operator, IExpression left, IExpression right) :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        left.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(@operator);
        builder.Append(' ');
        right.ToCode(ref builder);
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
        => ToCode(ref builder);
}