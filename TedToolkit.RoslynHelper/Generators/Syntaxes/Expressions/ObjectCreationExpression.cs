// -----------------------------------------------------------------------
// <copyright file="ObjectCreationExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Creation expression.
/// </summary>
/// <param name="dataType">data type.</param>
public sealed class ObjectCreationExpression(DataType? dataType = null) :
    IArguments,
    IExpression
{
    /// <inheritdoc />
    public List<Argument> Arguments
        => field ??= [];

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        if (dataType is not null)
        {
            builder.Append("new ");
            dataType.ToCode(ref builder);
        }
        else
        {
            builder.Append("new");
        }

        this.AddArgumentsNoSkip(ref builder);

        if (Variables.Count is 0)
            return;

        builder.BeginBlock();

        foreach (var (name, value) in Variables)
        {
            builder.AppendLine();
            builder.Append(name);
            builder.Append(" = ");
            value.ToCode(ref builder);
            builder.Append(',');
        }

        builder.EndBlock();
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
        => ToCode(ref builder);

    /// <summary>
    /// Gets some variables.
    /// </summary>
#pragma warning disable S2325
    public List<(string Name, IExpression Value)> Variables
#pragma warning restore S2325
        => field ??= [];

    /// <summary>
    /// Add variable.
    /// </summary>
    /// <param name="name">name.</param>
    /// <param name="value">value.</param>
    /// <returns>self.</returns>
    public ObjectCreationExpression AddVariable(string name, IExpression value)
    {
        Variables.Add((name, value));
        return this;
    }
}