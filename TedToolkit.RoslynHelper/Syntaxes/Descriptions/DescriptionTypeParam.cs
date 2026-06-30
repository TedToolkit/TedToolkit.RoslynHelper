// -----------------------------------------------------------------------
// <copyright file="DescriptionTypeParam.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Type Param Name.
/// </summary>
/// <param name="typePramName">paramName.</param>
/// <param name="descriptions">Descriptions.</param>
public sealed class DescriptionTypeParam(string typePramName, params IReadOnlyList<IDescriptionItem> descriptions) : IRootDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        if (descriptions.Count == 0)
        {
            return;
        }

        builder.Append("/// <typeparam name=\"");
        builder.Append(typePramName);
        builder.AppendLine("\">");
        descriptions.ToDescription(ref builder);
        builder.AppendLine("/// </typeparam>");
    }
}