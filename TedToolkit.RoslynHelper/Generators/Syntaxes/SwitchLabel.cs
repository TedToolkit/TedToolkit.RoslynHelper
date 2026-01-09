// -----------------------------------------------------------------------
// <copyright file="SwitchLabel.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The switch label
/// </summary>
/// <param name="value">case value</param>
/// <param name="when">when</param>
public sealed class SwitchLabel(IExpression? value = null, IExpression? when = null) :
    IToCode
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        if (value is null)
        {
            builder.Append("default:");
            return;
        }

        builder.Append("case ");
        value.ToCode(ref builder);
        if (when is not null)
        {
            builder.Append(" when ");
            when.ToCode(ref builder);
        }

        builder.Append(':');
    }
}