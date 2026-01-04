// -----------------------------------------------------------------------
// <copyright file="IToCode.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// For the codes.
/// </summary>
public interface IToCode
{
    /// <summary>
    /// Get the codes
    /// </summary>
    /// <returns>code</returns>
    string ToCode();
}