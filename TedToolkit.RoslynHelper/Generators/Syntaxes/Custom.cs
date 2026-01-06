// -----------------------------------------------------------------------
// <copyright file="Custom.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Delegates;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The custom codes
/// </summary>
/// <param name="Action">your action</param>
public record struct Custom(SourceBuilderHandler Action) :
    IStatement,
    IMember
{
    /// <inheritdoc />
    public readonly void ToCode(ref SourceBuilder builder)
        => Action(ref builder);

    /// <summary>
    /// Create by string
    /// </summary>
    /// <param name="value">the string</param>
    public Custom(string value)
        : this((ref b) => b.Append(value))
    {
    }
}