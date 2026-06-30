// -----------------------------------------------------------------------
// <copyright file="DescriptionBold.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Bold.
/// </summary>
/// <param name="descriptions">Descriptions.</param>
public sealed class DescriptionBold(params IReadOnlyList<IDescriptionItem> descriptions) : IDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        if (descriptions.Count == 0)
        {
            return;
        }

        builder.AppendLine("/// <b>");
        descriptions.ToDescription(ref builder);
        builder.AppendLine("/// </b>");
    }
}