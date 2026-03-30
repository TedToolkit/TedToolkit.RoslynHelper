// -----------------------------------------------------------------------
// <copyright file="Custom.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Delegates;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The custom codes.
/// </summary>
/// <param name="action">your action.</param>
public sealed class Custom(SourceBuilderHandler action) :
    IStatement,
    IMember
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        action(ref builder);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Custom"/> class.
    /// Create by string.
    /// </summary>
    /// <param name="value">the string.</param>
    public Custom(string value)
        : this((ref b) => b.Append(value))
    {
    }
}