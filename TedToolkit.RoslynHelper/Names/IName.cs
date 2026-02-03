// -----------------------------------------------------------------------
// <copyright file="IName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
/// The names.
/// </summary>
[Obsolete("Do not use this method, try to use the generators instead!")]
public interface IName
{
    /// <summary>
    ///     Gets full Name.
    /// </summary>
    string FullName { get; }

    /// <summary>
    ///     Gets full Name with Null.
    /// </summary>
    string FullNameNull { get; }

    /// <summary>
    ///     Gets summary Name.
    /// </summary>
    string SummaryName { get; }

    /// <summary>
    ///     Gets name.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Gets miniName.
    /// </summary>
    string MiniName { get; }

    /// <summary>
    ///     Gets full Name without global.
    /// </summary>
    string FullNameNoGlobal { get; }
}