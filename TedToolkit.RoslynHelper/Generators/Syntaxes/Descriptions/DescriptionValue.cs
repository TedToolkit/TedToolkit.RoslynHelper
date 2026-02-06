// -----------------------------------------------------------------------
// <copyright file="DescriptionValue.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Value.
/// </summary>
/// <param name="descriptions">Descriptions.</param>
public sealed class DescriptionValue(params IReadOnlyList<IDescriptionItem> descriptions) : IRootDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        if (descriptions.Count == 0)
        {
            return;
        }

        builder.AppendLine("/// <value>");
        descriptions.ToDescription(ref builder);
        builder.AppendLine("/// </value>");
    }
}