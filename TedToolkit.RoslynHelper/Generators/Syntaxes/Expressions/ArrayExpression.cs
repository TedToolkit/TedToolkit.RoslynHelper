// -----------------------------------------------------------------------
// <copyright file="ArrayExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The array expression
/// </summary>
/// <param name="expression">expression</param>
public sealed class ArrayExpression(IExpression expression) :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        expression.ToCode(ref builder);
        builder.Append("[]");
    }
}