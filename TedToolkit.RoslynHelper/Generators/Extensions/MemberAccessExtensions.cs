// -----------------------------------------------------------------------
// <copyright file="MemberAccessExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Types;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The member access extensions.
/// </summary>
public static class MemberAccessExtensions
{
#pragma warning disable CA1034
    extension(ref MemberAccess instance)
#pragma warning restore CA1034
    {
        /// <summary>
        /// The sub access.
        /// </summary>
        /// <param name="item">item.</param>
        /// <returns>access</returns>
#pragma warning disable S2325
        public ref MemberAccess Sub(MemberAccessItem item)
#pragma warning restore S2325
        {
            instance.Items.Add(item);
            return ref instance;
        }
    }
}