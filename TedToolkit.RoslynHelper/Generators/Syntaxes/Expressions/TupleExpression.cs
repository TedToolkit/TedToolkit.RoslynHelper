// -----------------------------------------------------------------------
// <copyright file="TupleExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Tuple expression
/// </summary>
public sealed class TupleExpression : IExpression
{
    private readonly List<(DataType Type, string Identifier)> _items = [];

    /// <summary>
    /// Add the item
    /// </summary>
    /// <param name="dataType">data type</param>
    /// <param name="identifier">identifier</param>
    /// <returns>self</returns>
    public TupleExpression AddItem(DataType dataType, string identifier = "")
    {
        _items.Add((dataType, identifier));
        return this;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append('(');
        var isNotStart = false;
        foreach (var (dataType, identifier) in _items.AsSpan())
        {
            if (isNotStart)
                builder.Append(", ");

            dataType.ToCode(ref builder);

            if (!string.IsNullOrEmpty(identifier))
            {
                builder.AppendSpace();
                builder.Append(identifier);
            }

            isNotStart = true;
        }

        builder.Append(')');
    }
}