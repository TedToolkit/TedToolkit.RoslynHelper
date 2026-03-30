// -----------------------------------------------------------------------
// <copyright file="NullExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// For the null expression.
/// </summary>
/// <param name="expression">the null expression.</param>
public sealed class NullExpression(IExpression expression) : IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        expression.ToCode(ref builder);
        builder.Append('?');
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        ToCode(ref builder);
    }
}