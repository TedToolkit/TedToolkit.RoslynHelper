// -----------------------------------------------------------------------
// <copyright file="IfStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The if statement.
/// </summary>
/// <param name="expression">expression.</param>
public sealed class IfStatement(IExpression expression) :
    IStatement,
    IStatementOwner
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("if (");
        expression.ToCode(ref builder);
        builder.Append(')');
        this.AddStatements(ref builder);
    }

    /// <inheritdoc/>
    public List<IStatement> Statements
        => field ??= [];
}