// -----------------------------------------------------------------------
// <copyright file="UsingStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// the using statement.
/// </summary>
/// <param name="expression">expression.</param>
public sealed class UsingStatement(IExpression expression) :
    ConditionalCompilationSyntax,
    IStatement,
    IAwait,
    IStatementOwner
{
    /// <inheritdoc />
    protected override void WriteSyntax(ref SourceBuilder builder)
    {
        this.AddAwait(ref builder);
        builder.Append("using (");
        expression.ToCode(ref builder);
        builder.Append(')');
        this.AddStatements(ref builder);
    }

    /// <inheritdoc />
    public bool IsAwait { get; set; }

    /// <inheritdoc />
    public List<IStatement> Statements
    {
        get
        {
            return field ??= [];
        }
    }
}