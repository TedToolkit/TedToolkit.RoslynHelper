// -----------------------------------------------------------------------
// <copyright file="UsingStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// the using statement
/// </summary>
/// <param name="type">type</param>
/// <param name="identifier">identifier</param>
/// <param name="expression">expression</param>
public sealed class UsingStatement(DataType type, string identifier, IExpression expression) :
    IStatement,
    IVariable,
    IStatementOwner
{
    private DataType _type = type;

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("using (");
        _type.ToCode(ref builder);
        builder.Append(" @");
        builder.Append(identifier);
        builder.Append(" = ");
        expression.ToCode(ref builder);
        builder.Append(')');
        this.AddStatements(ref builder);
    }

    /// <inheritdoc/>
    public string Variable
        => ZString.Concat('@', identifier);

    /// <inheritdoc />
    public List<IStatement> Statements
        => field ??= [];
}