// -----------------------------------------------------------------------
// <copyright file="DescriptionCode.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Code.
/// </summary>
/// <param name="descriptions">Descriptions.</param>
public sealed class DescriptionCode(params IReadOnlyList<IDescriptionItem> descriptions) : IDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        if (descriptions.Count == 0)
        {
            return;
        }

        builder.AppendLine("/// <code>");
        descriptions.ToDescription(ref builder);
        builder.AppendLine("/// </code>");
    }
}