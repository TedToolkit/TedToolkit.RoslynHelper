// -----------------------------------------------------------------------
// <copyright file="SimpleNameExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Types;

/// <summary>
/// Just a name Expression
/// </summary>
/// <param name="name">name of the expression</param>
public sealed class SimpleNameExpression(string name) :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
        => builder.Append(name);
}