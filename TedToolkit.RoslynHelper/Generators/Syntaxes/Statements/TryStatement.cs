// -----------------------------------------------------------------------
// <copyright file="TryStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Try statement.
/// </summary>
public sealed class TryStatement :
    IStatement,
    IStatementOwner
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("try");
        this.AddStatements(ref builder);
        foreach (var catchClause in Catches)
            catchClause.ToCode(ref builder);

        Finally?.ToCode(ref builder);
    }

    /// <inheritdoc />
    public List<IStatement> Statements
        => field ??= [];

    /// <summary>
    /// The catches.
    /// </summary>
#pragma warning disable SA1623
    public List<CatchClause> Catches
#pragma warning restore SA1623
        => field ??= [];

    /// <summary>
    /// Gets finally.
    /// </summary>
    public FinallyClause? Finally { get; private set; }

    /// <summary>
    /// Add the catch.
    /// </summary>
    /// <param name="catchClause">catch.</param>
    /// <returns>self.</returns>
    public TryStatement AddCatch(CatchClause catchClause)
    {
        Catches.Add(catchClause);
        return this;
    }

    /// <summary>
    /// Add the `finally`.
    /// </summary>
    /// <param name="finallyClause">finally.</param>
    /// <returns>self.</returns>
    public TryStatement AddFinally(FinallyClause finallyClause)
    {
        Finally = finallyClause;
        return this;
    }
}