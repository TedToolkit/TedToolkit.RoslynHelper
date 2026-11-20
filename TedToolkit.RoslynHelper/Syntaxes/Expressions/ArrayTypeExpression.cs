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
    /// Gets the immediate element type.
    /// </summary>
    internal IExpression ElementType { get; } = elementType;

    /// <summary>
    /// Gets the underlying element type through any nullable array layers.
    /// </summary>
    internal IExpression BaseElementType
    {
        get
        {
            var element = ElementType;
            while (Unwrap(element) is { } nested)
            {
                element = nested.ElementType;
            }

            return element;
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

    /// <summary>
    /// Replaces the underlying element while retaining any array layers and nullable wrappers.
    /// </summary>
    /// <param name="expression">The original type expression.</param>
    /// <param name="elementType">The replacement element type.</param>
    /// <returns>The resulting type expression.</returns>
    internal static IExpression WithBaseElementType(IExpression expression, IExpression elementType)
    {
        if (Unwrap(expression) is not { } array)
        {
            return elementType;
        }

        var updated = new ArrayTypeExpression(WithBaseElementType(array.ElementType, elementType), array.Rank);
        return expression is NullExpression ? updated.Null : updated;
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