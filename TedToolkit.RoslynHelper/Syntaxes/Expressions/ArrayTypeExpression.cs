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
/// <param name="isNullable">Whether this array layer is nullable.</param>
internal sealed class ArrayTypeExpression(IExpression elementType, int rank = 1, bool isNullable = false) : IExpression
{
    /// <summary>
    /// Gets or sets the immediate element type.
    /// </summary>
    internal IExpression ElementType { get; set; } = elementType;

    /// <summary>
    /// Gets the innermost consecutive array layer.
    /// </summary>
    internal ArrayTypeExpression InnermostLayer
    {
        get
        {
            var layer = this;
            while (layer.ElementType is ArrayTypeExpression nested)
            {
                layer = nested;
            }

            return layer;
        }
    }

    private int Rank { get; } = rank;

    private bool IsNullable { get; } = isNullable;

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        var last = this;
        while (last.ElementType is ArrayTypeExpression { IsNullable: false, } inner)
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

        if (!IsNullable)
        {
            return;
        }

        builder.Append('?');
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        ToCode(ref builder);
    }
}