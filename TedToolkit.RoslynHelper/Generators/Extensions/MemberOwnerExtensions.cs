// -----------------------------------------------------------------------
// <copyright file="MemberOwnerExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IMemberOwner"/>
/// </summary>
public static class MemberOwnerExtensions
{
#pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IMemberOwner
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add the member and indent.
        /// </summary>
        /// <param name="member">the member</param>
        /// <typeparam name="TMember">member type</typeparam>
        /// <returns>the item</returns>
        public ref TItem AddMember<TMember>(TMember member)
            where TMember : struct, IMember
        {
            instance.Members.Add(member.ToCode().Indent());
            return ref instance;
        }
    }
}