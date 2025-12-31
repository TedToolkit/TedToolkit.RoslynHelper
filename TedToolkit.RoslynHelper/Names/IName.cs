// -----------------------------------------------------------------------
// <copyright file="IName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
/// The names.
/// </summary>
public interface IName
{
    /// <summary>
    ///     Full Name
    /// </summary>
    string FullName { get; }

    /// <summary>
    ///     Full Name with Null
    /// </summary>
    string FullNameNull { get; }

    /// <summary>
    ///     Summary Name
    /// </summary>
    string SummaryName { get; }

    /// <summary>
    ///     Name
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     MiniName
    /// </summary>
    string MiniName { get; }

    /// <summary>
    ///     Full Name without global
    /// </summary>
    string FullNameNoGlobal { get; }
}