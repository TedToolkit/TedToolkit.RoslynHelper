// -----------------------------------------------------------------------
// <copyright file="HintNameConnectorType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// Controls how generated hint name segments are joined together.
/// </summary>
public enum HintNameConnectorType
{
    /// <summary>
    /// Keep the first connector that appears between segments.
    /// </summary>
    KEEP_FIRST = 0,

    /// <summary>
    /// Keep the last connector that appears between segments.
    /// </summary>
    KEEP_LAST = 1,

    /// <summary>
    /// Keep every connector that appears between segments.
    /// </summary>
    KEEP_ALL = 2,
}