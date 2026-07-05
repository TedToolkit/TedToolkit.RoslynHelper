// -----------------------------------------------------------------------
// <copyright file="ReturnStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The return statement.
/// </summary>
/// <param name="expression">expression.</param>
public sealed class ReturnStatement(IExpression? expression = null) : ConditionalCompilationStatement
{
    /// <inheritdoc/>
    protected override void WriteStatement(ref SourceBuilder builder)
    {
        if (expression is null)
        {
            builder.Append("return;");
            return;
        }

        builder.Append("return ");
        expression.ToCode(ref builder);
        builder.Append(";");
    }
}