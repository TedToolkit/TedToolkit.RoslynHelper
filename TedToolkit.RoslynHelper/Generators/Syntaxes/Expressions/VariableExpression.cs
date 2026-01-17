// -----------------------------------------------------------------------
// <copyright file="VariableExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Add the variable statement
/// </summary>
/// <param name="type">type</param>
/// <param name="identifier">identifier</param>
public sealed class VariableExpression(DataType type, string identifier) :
    IExpression,
    IVariable,
    IDefault,
    IConst
{
    /// <inheritdoc />
    public IExpression? Default { get; set; }

    /// <summary>
    /// Add the default value
    /// </summary>
    /// <param name="expression">expression</param>
    /// <returns>result</returns>
    public VariableExpression AddDefault(IExpression expression)
    {
        Default = expression;
        return this;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddConst(ref builder);
        type.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(identifier.ToValidIdentifier());
        this.AddDefault(ref builder);
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
        => ToCode(ref builder);

    /// <inheritdoc/>
    public string Variable
        => identifier.ToValidIdentifier();

    /// <inheritdoc/>
    public bool IsConst { get; set; }
}