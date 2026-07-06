// -----------------------------------------------------------------------
// <copyright file="DescriptionInheritDoc.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Inherit the doc.
/// </summary>
/// <param name="cref">cref.</param>
public sealed class DescriptionInheritDoc(ICref? cref = null) : IRootDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        if (cref is null)
        {
            builder.AppendLine("/// <inheritdoc/>");
            return;
        }

        builder.Append("/// <inheritdoc cref=\"");
        cref.ToCref(ref builder);
        builder.AppendLine("\"/>");
    }
}