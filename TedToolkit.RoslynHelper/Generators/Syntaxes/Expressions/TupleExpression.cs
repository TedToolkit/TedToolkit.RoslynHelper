// -----------------------------------------------------------------------
// <copyright file="TupleExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Tuple expression.
/// </summary>
public sealed class TupleExpression : IExpression
{
    private readonly List<IExpression> _items = [];

    /// <summary>
    /// Add the item.
    /// </summary>
    /// <param name="dataType">data type.</param>
    /// <param name="identifier">identifier.</param>
    /// <returns>self.</returns>
    public TupleExpression AddItem(DataType dataType, string identifier = "")
    {
        _items.Add(new VariableExpression(dataType, identifier));
        return this;
    }

    /// <summary>
    /// Add the item.
    /// </summary>
    /// <param name="expression">identifier.</param>
    /// <returns>self.</returns>
    public TupleExpression AddItem(IExpression expression)
    {
        _items.Add(expression);
        return this;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append('(');
        var isNotStart = false;
        foreach (var expression in _items.AsSpan())
        {
            if (isNotStart)
            {
                builder.Append(", ");
            }

            expression.ToCode(ref builder);

            isNotStart = true;
        }

        builder.Append(')');
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        ToCode(ref builder);
    }
}