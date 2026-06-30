// -----------------------------------------------------------------------
// <copyright file="Statement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Create a statement.
/// </summary>
/// <param name="expression">the expression.</param>
public sealed class Statement(IExpression expression) :
    IStatement
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        expression.ToCode(ref builder);
        builder.Append(';');
    }
}