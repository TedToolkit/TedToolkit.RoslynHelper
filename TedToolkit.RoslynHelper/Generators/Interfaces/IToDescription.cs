// -----------------------------------------------------------------------
// <copyright file="IToDescription.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// ToGet the description.
/// </summary>
public interface IToDescription
{
    /// <summary>
    /// Get the description.
    /// </summary>
    /// <param name="builder">builder.</param>
    void ToDescription(ref SourceBuilder builder);
}