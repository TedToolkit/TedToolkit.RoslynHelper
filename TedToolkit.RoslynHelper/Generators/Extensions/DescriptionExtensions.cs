// -----------------------------------------------------------------------
// <copyright file="DescriptionExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IDescription"/>.
/// </summary>
public static class DescriptionExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IDescription
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add description.
        /// </summary>
        /// <param name="description">description.</param>
        public TItem AddDescription(IDescriptionItem description)
        {
            instance.Descriptions.Add(description);
            return instance;
        }
    }

#pragma warning disable CA1034
    extension(IEnumerable<IToDescription> descriptions)
#pragma warning restore CA1034
#pragma warning disable S2325
    {
        /// <summary>
        /// 转为描述.
        /// </summary>
        /// <param name="builder">builder.</param>
        internal void ToDescription(ref SourceBuilder builder)
        {
            foreach (var descriptionItem in descriptions)
                descriptionItem.ToDescription(ref builder);
        }
    }
#pragma warning restore S2325
}