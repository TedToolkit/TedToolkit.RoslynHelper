// -----------------------------------------------------------------------
// <copyright file="IMemberOwner.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Delegates;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The owner of the member.
/// </summary>
public interface IMemberOwner
{
    /// <summary>
    /// The members
    /// </summary>
    List<ToCodeHandler> Members { get; }
}