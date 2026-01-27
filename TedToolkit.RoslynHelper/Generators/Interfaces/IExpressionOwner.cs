// -----------------------------------------------------------------------
// <copyright file="IExpressionOwner.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The expression.
/// </summary>
public interface IExpressionOwner
{
    /// <summary>
    /// Gets the members.
    /// </summary>
    List<IExpression> Expressions { get; }
}