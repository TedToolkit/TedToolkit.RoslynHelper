// -----------------------------------------------------------------------
// <copyright file="ConditionalItem.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Wraps a syntax item together with an optional conditional compilation expression.
/// </summary>
/// <typeparam name="T">The wrapped item type.</typeparam>
/// <param name="item">The wrapped item.</param>
/// <param name="condition">The optional conditional compilation expression.</param>
public sealed class ConditionalItem<T>(T item, PreprocessorExpression? condition = null)
{
    /// <summary>
    /// Gets the wrapped item.
    /// </summary>
    public T Item { get; } = item;

    /// <summary>
    /// Gets or sets the conditional compilation expression.
    /// </summary>
    public PreprocessorExpression? Condition { get; set; } = condition;

#pragma warning disable CA1000, CA2225

    /// <summary>
    /// Wraps an item and optional condition explicitly.
    /// </summary>
    /// <param name="item">The item to wrap.</param>
    /// <param name="condition">The optional conditional compilation expression.</param>
    /// <returns>The wrapped item.</returns>
    public static ConditionalItem<T> From(T item, PreprocessorExpression? condition = null)
    {
        return new(item, condition);
    }

    /// <summary>
    /// Implicitly wraps an item without a condition.
    /// </summary>
    /// <param name="item">The item to wrap.</param>
    public static implicit operator ConditionalItem<T>(T item)
    {
        return From(item);
    }
#pragma warning restore CA1000, CA2225

    /// <summary>
    /// Deconstructs the wrapper into item and condition components.
    /// </summary>
    /// <param name="item">The wrapped item.</param>
    /// <param name="condition">The optional conditional compilation expression.</param>
    public void Deconstruct(out T item, out PreprocessorExpression? condition)
    {
        item = Item;
        condition = Condition;
    }
}