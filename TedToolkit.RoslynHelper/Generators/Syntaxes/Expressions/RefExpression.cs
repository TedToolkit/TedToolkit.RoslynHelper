// -----------------------------------------------------------------------
// <copyright file="RefExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Make the expression add with ref.
/// </summary>
/// <param name="expression">expression.</param>
public sealed class RefExpression(IExpression expression) : IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("ref ");
        expression.ToCode(ref builder);
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        ToCode(ref builder);
    }
}