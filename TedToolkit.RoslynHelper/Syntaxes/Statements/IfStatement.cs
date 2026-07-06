// -----------------------------------------------------------------------
// <copyright file="IfStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The if statement.
/// </summary>
/// <param name="expression">expression.</param>
public sealed class IfStatement(IExpression expression) :
    ConditionalCompilationSyntax,
    IStatement,
    IStatementOwner
{
    private readonly IExpression _expression = expression;

    private List<ConditionalBranch>? _branches;

    private ElseBranch? _elseBranch;

    private IStatementOwner? _currentBranch;

    /// <summary>
    /// Adds a statement to the current branch.
    /// </summary>
    /// <param name="statement">statement.</param>
    /// <typeparam name="TStatement">statement type.</typeparam>
    /// <returns>this.</returns>
    public IfStatement AddStatement<TStatement>(TStatement statement)
        where TStatement : class, IStatement
    {
        this.GetCurrentBranch().Statements.Add(statement);
        return this;
    }

    /// <summary>
    /// Adds a statement expression to the current branch.
    /// </summary>
    /// <param name="expression">expression.</param>
    /// <returns>this.</returns>
    public IfStatement AddStatement(IExpression expression)
    {
        this.GetCurrentBranch().Statements.Add(new Statement(expression));
        return this;
    }

    /// <summary>
    /// Starts an else-if branch.
    /// </summary>
    /// <param name="expression">expression.</param>
    /// <returns>this.</returns>
    /// <exception cref="InvalidOperationException">else branch already exists.</exception>
    public IfStatement ElseIf(IExpression expression)
    {
        if (this._elseBranch is not null)
        {
            throw new InvalidOperationException("Cannot add an else-if branch after an else branch.");
        }

        var branch = new ConditionalBranch(expression);
        this._branches ??= [];
        this._branches.Add(branch);
        this._currentBranch = branch;
        return this;
    }

    /// <summary>
    /// Starts an else branch.
    /// </summary>
    /// <returns>this.</returns>
    /// <exception cref="InvalidOperationException">else branch already exists.</exception>
    public IfStatement Else()
    {
        if (this._elseBranch is not null)
        {
            throw new InvalidOperationException("Cannot add more than one else branch.");
        }

        this._elseBranch = new();
        this._currentBranch = this._elseBranch;
        return this;
    }

    /// <inheritdoc/>
    protected override void WriteSyntax(ref SourceBuilder builder)
    {
        builder.Append("if (");
        _expression.ToCode(ref builder);
        builder.Append(')');
        this.AddStatements(ref builder);

        if (this._branches is not null)
        {
            foreach (var branch in this._branches)
            {
                builder.AppendLine();
                builder.Append("else if (");
                branch.Expression.ToCode(ref builder);
                builder.Append(')');
                branch.AddStatements(ref builder);
            }
        }

        if (this._elseBranch is null)
        {
            return;
        }

        builder.AppendLine();
        builder.Append("else");
        this._elseBranch.AddStatements(ref builder);
    }

    /// <inheritdoc/>
    public List<IStatement> Statements
    {
        get
        {
            return field ??= [];
        }
    }

    private IStatementOwner GetCurrentBranch()
    {
        return this._currentBranch ?? this;
    }

    private sealed class ConditionalBranch(IExpression expression) : IStatementOwner
    {
        public IExpression Expression { get; } = expression;

        public List<IStatement> Statements
        {
            get
            {
                return field ??= [];
            }
        }
    }

    private sealed class ElseBranch : IStatementOwner
    {
        public List<IStatement> Statements
        {
            get
            {
                return field ??= [];
            }
        }
    }
}