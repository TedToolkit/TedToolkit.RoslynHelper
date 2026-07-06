// -----------------------------------------------------------------------
// <copyright file="MemberOwnerExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The extensions for the <see cref="IMemberOwner"/>.
/// </summary>
public static class MemberOwnerExtensions
{
    /// <summary>
    /// Get the type name.
    /// </summary>
    /// <param name="type">type.</param>
    /// <returns>string.</returns>
#pragma warning disable S3398
    private static string GetTypeName(TypeDeclaration type)
#pragma warning restore S3398
    {
        var name = type.Identifier;
        if (type.TypeParameters.Count is 0)
        {
            return name;
        }

        using var builder = ZString.CreateStringBuilder();
        builder.Append(name);
        builder.Append('<');

        var isNotStart = false;

        foreach (var typeTypeParameter in type.TypeParameters)
        {
            if (isNotStart)
            {
                builder.Append(", ");
            }

            builder.Append(typeTypeParameter.Variable);
            isNotStart = true;
        }

        builder.Append('>');
        return builder.ToString();
    }

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
            {
                owner.Owner = GetTypeName(type);
            }

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
                {
                    builder.AppendLine();
                }

                member.ToCode(ref builder);
                isNotStart = true;
            }

            builder.EndBlock();
        }
    }
}