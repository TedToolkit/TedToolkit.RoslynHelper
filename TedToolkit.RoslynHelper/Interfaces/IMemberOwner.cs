// -----------------------------------------------------------------------
// <copyright file="IMemberOwner.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The owner of the member.
/// </summary>
public interface IMemberOwner
{
    /// <summary>
    /// Gets the members.
    /// </summary>
    List<IMember> Members { get; }
}