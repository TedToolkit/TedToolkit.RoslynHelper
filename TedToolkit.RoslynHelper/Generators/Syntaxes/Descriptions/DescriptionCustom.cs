// -----------------------------------------------------------------------
// <copyright file="DescriptionCustom.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Delegates;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The custom description.
/// </summary>
/// <param name="handler">handler.</param>
public class DescriptionCustom(SourceBuilderHandler handler) :
    IDescriptionItem,
    IRootDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
        => handler(ref builder);

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptionCustom"/> class.
    /// Create by string.
    /// </summary>
    /// <param name="value">the string.</param>
    public DescriptionCustom(string value)
        : this((ref b) => b.Append(value))
    {
    }
}