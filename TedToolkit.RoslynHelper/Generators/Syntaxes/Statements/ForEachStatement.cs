// -----------------------------------------------------------------------
// <copyright file="ForEachStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The Foreach Statement
/// </summary>
/// <param name="type">type</param>
/// <param name="identifier">identifier</param>
/// <param name="expression">expression</param>
public sealed class ForEachStatement(DataType type, string identifier, IExpression expression) :
    IStatement,
    IVariable,
    IStatementOwner
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("for (");
        type.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(identifier.ToValidIdentifier());
        builder.Append(" in ");
        expression.ToCode(ref builder);
        builder.Append(')');
        this.AddStatements(ref builder);
    }

    /// <inheritdoc/>
    public string Variable
        => identifier.ToValidIdentifier();

    /// <inheritdoc/>
    public List<IStatement> Statements
        => field ??= [];
}