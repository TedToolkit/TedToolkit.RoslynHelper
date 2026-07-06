// -----------------------------------------------------------------------
// <copyright file="DescriptionCode.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Code.
/// </summary>
/// <param name="block">is a block</param>
/// <param name="descriptions">Descriptions.</param>
public sealed class DescriptionCode(bool block, params IReadOnlyList<IDescriptionItem> descriptions) : IDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        if (descriptions.Count == 0)
        {
            return;
        }

        builder.AppendLine(block ? "/// <code>" : "/// <c>");
        descriptions.ToDescription(ref builder);
        builder.AppendLine(block ? "/// </code>" : "/// </c>");
    }
}