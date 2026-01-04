// -----------------------------------------------------------------------
// <copyright file="MemberAccessItemExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Types;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The member access extensions.
/// </summary>
public static class MemberAccessItemExtensions
{
#pragma warning disable CA1034
    extension(ref MemberAccessItem instance)
#pragma warning restore CA1034
    {
        /// <summary>
        /// The sub access.
        /// </summary>
        /// <param name="type">item.</param>
        /// <returns>access</returns>
#pragma warning disable S2325
        public ref MemberAccessItem AddType(MemberAccess type)
#pragma warning restore S2325
        {
            instance.Types.Add(type);
            return ref instance;
        }
    }
}