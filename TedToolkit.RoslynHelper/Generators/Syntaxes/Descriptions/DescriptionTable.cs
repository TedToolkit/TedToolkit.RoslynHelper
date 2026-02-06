// -----------------------------------------------------------------------
// <copyright file="DescriptionTable.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The description table.
/// </summary>
/// <param name="termHeader">term header.</param>
/// <param name="descriptionHeader">description header.</param>
public sealed class DescriptionTable(IDescriptionItem termHeader, IDescriptionItem descriptionHeader) : IDescriptionItem
{
    /// <summary>
    /// Gets the list items.
    /// </summary>
    public List<(IDescriptionItem Term, IDescriptionItem Description)> Items { get; } = [];

    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        builder.AppendLine("/// <list type=\"table\">");
        AppendHeader(ref builder, termHeader, descriptionHeader);
        foreach (var (term, description) in Items)
        {
            AppendItem(ref builder, term, description);
        }

        builder.AppendLine("/// </list>");
    }

    /// <summary>
    /// Add an item.
    /// </summary>
    /// <param name="term">term.</param>
    /// <param name="description">description.</param>
    /// <returns>self.</returns>
    public DescriptionTable AddItem(IDescriptionItem term, IDescriptionItem description)
    {
        Items.Add((term, description));
        return this;
    }

    private static void AppendItem(ref SourceBuilder builder, IDescriptionItem term, IDescriptionItem description)
    {
        builder.AppendLine("/// <item>");
        AppendTerm(ref builder, term);
        AppendDescription(ref builder, description);
        builder.AppendLine("/// </item>");
    }

    private static void AppendHeader(ref SourceBuilder builder, IDescriptionItem term, IDescriptionItem description)
    {
        builder.AppendLine("/// <listheader>");
        AppendTerm(ref builder, term);
        AppendDescription(ref builder, description);
        builder.AppendLine("/// </listheader>");
    }

    private static void AppendTerm(ref SourceBuilder builder, IDescriptionItem item)
    {
        builder.AppendLine("/// <term>");
        item.ToDescription(ref builder);
        builder.AppendLine("/// </term>");
    }

    private static void AppendDescription(ref SourceBuilder builder, IDescriptionItem item)
    {
        builder.AppendLine("/// <description>");
        item.ToDescription(ref builder);
        builder.AppendLine("/// </description>");
    }
}