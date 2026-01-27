// -----------------------------------------------------------------------
// <copyright file="ICref.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// For the item that can be a cref.
/// </summary>
public interface ICref
{
    /// <summary>
    /// Get the cref.
    /// </summary>
    /// <param name="builder">builder.</param>
    void ToCref(ref SourceBuilder builder);
}