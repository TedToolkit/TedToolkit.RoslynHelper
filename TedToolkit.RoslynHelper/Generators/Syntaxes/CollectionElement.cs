// -----------------------------------------------------------------------
// <copyright file="CollectionElement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The collection elements.
/// </summary>
/// <param name="expression">expression.</param>
public sealed class CollectionElement(IExpression expression) :
    IToCode
{
    /// <summary>
    /// Gets or sets a value indicating whether if it is spread.
    /// </summary>
    public bool IsSpread { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        if (IsSpread)
        {
            builder.Append("..");
        }

        expression.ToCode(ref builder);
    }

    /// <summary>
    /// Gets spread it.
    /// </summary>
    public CollectionElement Spread
    {
        get
        {
            IsSpread = true;
            return this;
        }
    }
}