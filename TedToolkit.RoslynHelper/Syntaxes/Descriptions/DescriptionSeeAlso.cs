// -----------------------------------------------------------------------
// <copyright file="DescriptionSeeAlso.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// SeeAlso.
/// </summary>
/// <param name="cref">cref.</param>
public sealed class DescriptionSeeAlso(ICref cref) : IRootDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        builder.Append("/// <seealso cref=\"");
        cref.ToCref(ref builder);
        builder.AppendLine("\"/>");
    }
}