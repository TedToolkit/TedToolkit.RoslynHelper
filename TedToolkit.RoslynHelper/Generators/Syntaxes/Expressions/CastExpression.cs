// -----------------------------------------------------------------------
// <copyright file="CastExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Cast to the type
/// </summary>
/// <param name="type">type</param>
/// <param name="expression">expression</param>
public sealed class CastExpression(DataType type, IExpression expression) :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append('(');
        type.ToCode(ref builder);
        builder.Append(')');
        expression.ToCode(ref builder);
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
        => ToCode(ref builder);
}