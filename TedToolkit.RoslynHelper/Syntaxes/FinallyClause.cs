// -----------------------------------------------------------------------
// <copyright file="FinallyClause.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The `finally` clause.
/// </summary>
public sealed class FinallyClause :
    IStatementOwner,
    IToCode
{
    /// <inheritdoc/>
    public List<IStatement> Statements
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("finally");
        this.AddStatements(ref builder);
    }
}