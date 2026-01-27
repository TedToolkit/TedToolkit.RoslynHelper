// -----------------------------------------------------------------------
// <copyright file="CustomExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Delegates;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Custom expression.
/// </summary>
/// <param name="action">action.</param>
public sealed class CustomExpression(SourceBuilderHandler action) :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
        => action(ref builder);

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomExpression"/> class.
    /// Create by string.
    /// </summary>
    /// <param name="value">the string.</param>
    public CustomExpression(string value)
        : this((ref b) => b.Append(value))
    {
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
        => ToCode(ref builder);
}