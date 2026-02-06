// -----------------------------------------------------------------------
// <copyright file="DescriptionParam.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Summary.
/// </summary>
/// <param name="pramName">paramName.</param>
/// <param name="descriptions">Descriptions.</param>
public sealed class DescriptionParam(string pramName, params IReadOnlyList<IDescriptionItem> descriptions) : IRootDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        if (descriptions.Count == 0)
        {
            return;
        }

        builder.Append("/// <param name=\"");
        builder.Append(pramName);
        builder.AppendLine("\">");
        descriptions.ToDescription(ref builder);
        builder.AppendLine("/// </param>");
    }
}