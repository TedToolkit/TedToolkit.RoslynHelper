// -----------------------------------------------------------------------
// <copyright file="IStatementOwner.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The owner of the statements.
/// </summary>
public interface IStatementOwner
{
    /// <summary>
    /// The members
    /// </summary>
    List<IStatement> Statements { get; }
}