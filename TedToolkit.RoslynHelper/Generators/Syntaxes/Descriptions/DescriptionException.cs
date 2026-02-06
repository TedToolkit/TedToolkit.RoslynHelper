// -----------------------------------------------------------------------
// <copyright file="DescriptionException.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The exception description.
/// </summary>
/// <param name="exception">exception.</param>
/// <param name="descriptions">descriptions.</param>
#pragma warning disable S2166, CA1711
public sealed class DescriptionException(ICref exception, params IReadOnlyList<IDescriptionItem> descriptions) : IRootDescriptionItem
#pragma warning restore S2166, CA1711
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        if (descriptions.Count == 0)
        {
            return;
        }

        builder.Append("/// <exception cref=\"");
        exception.ToCref(ref builder);
        builder.AppendLine("\">");
        descriptions.ToDescription(ref builder);
        builder.AppendLine("/// </exception>");
    }
}