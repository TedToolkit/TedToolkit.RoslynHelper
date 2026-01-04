// -----------------------------------------------------------------------
// <copyright file="AttributeExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="TedToolkit.RoslynHelper.Generators.Types.Attribute"/>
/// </summary>
public static class AttributeExtensions
{
#pragma warning disable CA1034
    extension(ref TedToolkit.RoslynHelper.Generators.Types.Attribute instance)
#pragma warning restore CA1034
#pragma warning disable S2325
    {
        /// <summary>
        /// Add modifier
        /// </summary>
        /// <param name="modifier">modifier</param>
        /// <returns>the item</returns>
        public ref TedToolkit.RoslynHelper.Generators.Types.Attribute AddModifier(AttributeModifier modifier)
        {
            instance.Modifier = modifier;
            return ref instance;
        }
    }
#pragma warning restore S2325
}