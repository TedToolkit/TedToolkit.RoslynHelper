// -----------------------------------------------------------------------
// <copyright file="MemberOwnerExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom.Compiler;

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IMemberOwner"/>.
/// </summary>
public static class MemberOwnerExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IMemberOwner
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add the member.
        /// </summary>
        /// <param name="member">the member.</param>
        /// <typeparam name="TMember">member type.</typeparam>
        /// <returns>the item.</returns>
        public TItem AddMember<TMember>(TMember member)
            where TMember : class, IMember
        {
            if (member is IOwner owner && instance is TypeDeclaration type)
                owner.Owner = type.Identifier;

            instance.Members.Add(member);
            return instance;
        }

        internal void AddMembers(ref SourceBuilder builder)
        {
            if (instance.Members.Count is 0)
            {
                builder.Append(';');
                return;
            }

            builder.BeginBlock();

            var isNotStart = false;
            foreach (var member in instance.Members)
            {
                builder.AppendLine();
                if (isNotStart)
                    builder.AppendLine();

                member.ToCode(ref builder);
                isNotStart = true;
            }

            builder.EndBlock();
        }
    }
}