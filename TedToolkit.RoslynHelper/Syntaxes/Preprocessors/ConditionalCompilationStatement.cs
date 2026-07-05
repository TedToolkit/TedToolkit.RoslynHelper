// -----------------------------------------------------------------------
// <copyright file="ConditionalCompilationStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

/// <summary>
/// A conditional compilation block that renders statements.
/// </summary>
/// <param name="condition">condition.</param>
public sealed class ConditionalCompilationStatement(PreprocessorExpression condition) :
    ConditionalCompilationBlock<IStatement>(condition),
    IStatement
{
    /// <summary>
    /// Adds a statement to the current branch.
    /// </summary>
    /// <param name="statement">statement.</param>
    /// <typeparam name="TStatement">statement type.</typeparam>
    /// <returns>this.</returns>
    public ConditionalCompilationStatement AddStatement<TStatement>(TStatement statement)
        where TStatement : class, IStatement
    {
        CurrentItems.Add(statement);
        return this;
    }

    /// <summary>
    /// Adds an expression statement to the current branch.
    /// </summary>
    /// <param name="expression">expression.</param>
    /// <returns>this.</returns>
    public ConditionalCompilationStatement AddStatement(IExpression expression)
    {
        CurrentItems.Add(new Statement(expression));
        return this;
    }

    /// <summary>
    /// Starts an <c>#elif</c> branch.
    /// </summary>
    /// <param name="condition">condition.</param>
    /// <returns>this.</returns>
    public ConditionalCompilationStatement ElseIf(PreprocessorExpression condition)
    {
        StartElseIf(condition);
        return this;
    }

    /// <summary>
    /// Starts an <c>#else</c> branch.
    /// </summary>
    /// <returns>this.</returns>
    public ConditionalCompilationStatement Else()
    {
        StartElse();
        return this;
    }

    /// <summary>
    /// Gets the statements in the current branch.
    /// </summary>
    public List<IStatement> Statements
    {
        get
        {
            return CurrentItems;
        }
    }

    /// <inheritdoc />
    protected override void WriteItems(ref SourceBuilder builder, List<IStatement> items)
    {
        var isNotStart = false;
        foreach (var item in items)
        {
            if (isNotStart)
            {
                builder.AppendLine();
            }

            item.ToCode(ref builder);
            isNotStart = true;
        }
    }
}