// -----------------------------------------------------------------------
// <copyright file="ArrayTypeExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// One array layer around an element type, which can itself be an array.
/// </summary>
/// <param name="elementType">The immediate element type.</param>
/// <param name="rank">The number of dimensions in this layer.</param>
internal sealed class ArrayTypeExpression(IExpression elementType, int rank = 1) : IExpression
{
    /// <summary>
    /// Gets or sets the immediate element type.
    /// </summary>
    internal IExpression ElementType { get; set; } = elementType;

    /// <summary>
    /// Gets the innermost array layer through any nullable wrappers.
    /// </summary>
    internal ArrayTypeExpression InnermostLayer
    {
        get
        {
            var layer = this;
            while (Unwrap(layer.ElementType) is { } nested)
            {
                layer = nested;
            }

            return layer;
        }
    }

    private int Rank { get; } = rank;

    /// <summary>
    /// Finds an array expression, including one wrapped in a nullable modifier.
    /// </summary>
    /// <param name="expression">The expression to inspect.</param>
    /// <returns>The array expression, if present.</returns>
    internal static ArrayTypeExpression? Unwrap(IExpression expression)
    {
        return expression switch
        {
            ArrayTypeExpression array => array,
            NullExpression { Operand: ArrayTypeExpression array, } => array,
            _ => null,
        };
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        var last = this;
        while (last.ElementType is ArrayTypeExpression inner)
        {
            last = inner;
        }

        // A nullable inner array is emitted as an element before this group of ranks.
        last.ElementType.ToCode(ref builder);
        for (var layer = this; ; layer = (ArrayTypeExpression)layer.ElementType)
        {
            builder.Append('[');
            builder.Append(',', layer.Rank - 1);
            builder.Append(']');
            if (ReferenceEquals(layer, last))
            {
                break;
            }
        }
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        ToCode(ref builder);
    }
}