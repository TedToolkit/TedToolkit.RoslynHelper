// -----------------------------------------------------------------------
// <copyright file="CatchClause.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The catch clause.
/// </summary>
/// <param name="dataType">data type.</param>
/// <param name="identifier">identifier.</param>
public sealed class CatchClause(DataType dataType, string identifier) :
    IStatementOwner,
    IVariable,
    IToCode
{
    /// <inheritdoc/>
    public string Variable
        => identifier.ToValidIdentifier();

    /// <inheritdoc/>
    public List<IStatement> Statements
        => field ??= [];

    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("catch(");
        dataType.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(Variable);
        builder.Append(')');
        this.AddStatements(ref builder);
    }
}