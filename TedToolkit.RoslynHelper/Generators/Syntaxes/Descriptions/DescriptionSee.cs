// -----------------------------------------------------------------------
// <copyright file="DescriptionSee.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// See
/// </summary>
/// <param name="cref">cref</param>
public sealed class DescriptionSee(ICref cref) : IDescriptionItem
{
    /// <inheritdoc />
    public void ToDescription(ref SourceBuilder builder)
    {
        builder.Append("/// <see cref=\"");
        cref.ToCref(ref builder);
        builder.AppendLine("\"/>");
    }
}