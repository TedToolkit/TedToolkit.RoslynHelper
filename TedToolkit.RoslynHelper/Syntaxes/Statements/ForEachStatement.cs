// -----------------------------------------------------------------------
// <copyright file="ForEachStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The Foreach Statement.
/// </summary>
/// <param name="type">type.</param>
/// <param name="identifier">identifier.</param>
/// <param name="expression">expression.</param>
public sealed class ForEachStatement(DataType type, string identifier, IExpression expression) :
    ConditionalCompilationSyntax,
    IStatement,
    IAwait,
    IVariable,
    IStatementOwner
{
    /// <inheritdoc/>
    protected override void WriteSyntax(ref SourceBuilder builder)
    {
        this.AddAwait(ref builder);
        builder.Append("foreach (");
        type.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(identifier.ToValidIdentifier());
        builder.Append(" in ");
        expression.ToCode(ref builder);
        builder.Append(')');
        this.AddStatements(ref builder);
    }

    /// <inheritdoc />
    public bool IsAwait { get; set; }

    /// <inheritdoc/>
    public string Variable
    {
        get
        {
            return identifier.ToValidIdentifier();
        }
    }

    /// <inheritdoc/>
    public List<IStatement> Statements
    {
        get
        {
            return field ??= [];
        }
    }
}