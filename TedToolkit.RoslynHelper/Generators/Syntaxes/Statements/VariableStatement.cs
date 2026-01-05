// -----------------------------------------------------------------------
// <copyright file="VariableStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Add the variable statement
/// </summary>
/// <param name="type">type</param>
/// <param name="identifier">identifier</param>
public sealed class VariableStatement(DataType type, string identifier) :
    IStatement,
    IVariable
{
    private DataType _type = type;

    /// <summary>
    /// Default Value
    /// </summary>
    public IExpression? Default { get; set; }

    /// <summary>
    /// Add the default value
    /// </summary>
    /// <param name="expression">expression</param>
    /// <returns>result</returns>
    public VariableStatement AddDefault(IExpression expression)
    {
        Default = expression;
        return this;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        _type.ToCode(ref builder);
        builder.Append(" @");
        builder.Append(identifier);
        if (Default is null)
        {
            builder.Append(';');
            return;
        }

        builder.Append(" = ");
        Default.ToCode(ref builder);
        builder.Append(';');
    }

    /// <inheritdoc/>
    public string Variable
        => ZString.Concat('@', identifier);
}