// -----------------------------------------------------------------------
// <copyright file="RootDescriptionExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IRootDescription"/>.
/// </summary>
public static class RootDescriptionExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IRootDescription
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add description.
        /// </summary>
        /// <param name="description">description.</param>
        public TItem AddRootDescription(IRootDescriptionItem description)
        {
            instance.RootDescriptions.Add(description);
            return instance;
        }

        internal void AddDescriptions(ref SourceBuilder builder)
        {
            instance.RootDescriptions.ToDescription(ref builder);
        }
    }
}