// -----------------------------------------------------------------------
// <copyright file="AliasExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Types;

/// <summary>
/// The alias expression
/// </summary>
/// <param name="alias">alias</param>
/// <param name="expression">expression</param>
public sealed class AliasExpression(string alias, IExpression expression) :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append(alias);
        builder.Append("::");
        expression.ToCode(ref builder);
    }
}