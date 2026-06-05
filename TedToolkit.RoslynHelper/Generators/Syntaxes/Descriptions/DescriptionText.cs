// -----------------------------------------------------------------------
// <copyright file="DescriptionText.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// A simple text.
/// </summary>
/// <param name="value">text value.</param>
public sealed class DescriptionText(string value) : IDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        builder.Append("/// ");
        builder.AppendLine(value
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;")
            .ToValidLiteral());
    }
}