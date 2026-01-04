// -----------------------------------------------------------------------
// <copyright file="DescriptionExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

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
            instance.Description.Add(description);
            return ref instance;
        }

        internal void AddSummary(ref Utf16ValueStringBuilder builder)
        {
            if (instance.Description.Count == 0)
                return;

            builder.AppendLine("/// <summary>");
            instance.AddDescriptionItems(ref builder);

            builder.AppendLine("/// </summary>");
        }

        internal void AddDescriptionItems(ref Utf16ValueStringBuilder builder)
        {
            foreach (var descriptionItem in instance.Description)
                builder.AppendLine(descriptionItem.ToSummary());
        }
    }
}