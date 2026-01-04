// -----------------------------------------------------------------------
// <copyright file="DescriptionExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IDescription"/>
/// </summary>
public static class DescriptionExtensions
{
#pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IDescription
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add description
        /// </summary>
        /// <param name="description">description</param>
        public ref TItem AddDescription(string description)
        {
            instance.Description.DescriptionItems.Add(description);
            return ref instance;
        }
    }
}