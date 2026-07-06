// -----------------------------------------------------------------------
// <copyright file="CollectionExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The collection expression.
/// </summary>
public sealed class CollectionExpression :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append('[');
        builder.Indent();

        foreach (var expression in Elements)
        {
            builder.AppendLine();
            expression.ToCode(ref builder);
            builder.Append(',');
        }

        builder.Dedent();
        builder.AppendLine();
        builder.Append(']');
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
    }

    /// <summary>
    /// Gets the elements.
    /// </summary>
#pragma warning disable S2325
    public List<CollectionElement> Elements
#pragma warning restore S2325
        => field ??= [];

    /// <summary>
    /// Add one element.
    /// </summary>
    /// <param name="element">the element.</param>
    /// <returns>self.</returns>
    public CollectionExpression AddElement(CollectionElement element)
    {
        Elements.Add(element);
        return this;
    }

    /// <summary>
    /// Add one element.
    /// </summary>
    /// <param name="expression">the expression.</param>
    /// <param name="isSpread">is spread.</param>
    /// <returns>self.</returns>
    public CollectionExpression AddElement(IExpression expression, bool isSpread = false)
    {
        Elements.Add(new CollectionElement(expression) { IsSpread = isSpread, });
        return this;
    }
}