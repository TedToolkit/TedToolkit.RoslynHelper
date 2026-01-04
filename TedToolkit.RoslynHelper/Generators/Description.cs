// -----------------------------------------------------------------------
// <copyright file="Description.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The Description
/// </summary>
public record struct Description :
    IToDescription
{
    /// <summary>
    /// The description items.
    /// </summary>
#pragma warning disable S2325
    internal List<string> DescriptionItems
#pragma warning restore S2325
        => field ??= [];

    /// <inheritdoc />
    public string ToDescription()
    {
        using var builder = ZString.CreateStringBuilder();
        foreach (var descriptionItem in DescriptionItems)
            builder.Append(descriptionItem.ToSummary());

        return builder.ToString();
    }
}