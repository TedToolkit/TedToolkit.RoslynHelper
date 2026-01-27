// -----------------------------------------------------------------------
// <copyright file="IDescription.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The item that has description.
/// </summary>
public interface IDescription
{
    /// <summary>
    /// Gets the Description.
    /// </summary>
    List<IDescriptionItem> Descriptions { get; }

    /// <summary>
    /// Change to the root.
    /// </summary>
    /// <returns>root item.</returns>
    IRootDescriptionItem ToRoot();
}