// -----------------------------------------------------------------------
// <copyright file="ForEachStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes.Statements;

/// <summary>
/// The Foreach Statement
/// </summary>
/// <param name="type">type</param>
/// <param name="identifier">identifier</param>
/// <param name="expression">expression</param>
public sealed class ForEachStatement(IExpression type, string identifier, IExpression expression) :
    IStatement,
    IVariables,
    IStatementOwner
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("for (");
        type.ToCode(ref builder);
        builder.Append(" @");
        builder.Append(identifier);
        builder.Append(" in ");
        expression.ToCode(ref builder);
        builder.Append(')');
        this.AddStatements(ref builder);
    }

    /// <inheritdoc/>
    public string Variable
        => ZString.Concat('@', identifier);

    /// <inheritdoc/>
    public List<IStatement> Statements
        => field ??= [];
}