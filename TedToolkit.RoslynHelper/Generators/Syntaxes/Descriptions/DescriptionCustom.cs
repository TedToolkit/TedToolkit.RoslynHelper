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
/// <param name="handler">handler</param>
public class DescriptionCustom(SourceBuilderHandler handler) :
    IDescriptionItem,
    IRootDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
        => handler(ref builder);
}