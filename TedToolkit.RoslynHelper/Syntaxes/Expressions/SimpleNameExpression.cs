// -----------------------------------------------------------------------
// <copyright file="SimpleNameExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Just a name Expression.
/// </summary>
/// <param name="name">name of the expression.</param>
public sealed class SimpleNameExpression(string name) :
    IExpression
{
    /// <summary>
    /// Gets <see langword="null"/>.
    /// </summary>
    public static SimpleNameExpression Null { get; } = new("null");

    /// <summary>
    /// Gets <see langword="default"/>.
    /// </summary>
    public static SimpleNameExpression Default { get; } = new("default");

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append(name);
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        ToCode(ref builder);
    }
}