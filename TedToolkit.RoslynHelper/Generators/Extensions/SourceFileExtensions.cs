// -----------------------------------------------------------------------
// <copyright file="SourceFileExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Types;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="SourceFile"/>
/// </summary>
public static class SourceFileExtensions
{
#pragma warning disable CA1034
    extension(ref SourceFile instance)
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add a name space.
        /// </summary>
        /// <param name="nameSpace">the namespace</param>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref SourceFile AddNameSpace(NameSpace nameSpace)
        {
            instance.NameSpaces.Add(nameSpace);
            return ref instance;
        }

        /// <summary>
        /// Add a name space.
        /// </summary>
        /// <param name="attribute">the attribute</param>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref SourceFile AddAttribute(Types.Attribute attribute)
        {
            attribute.Modifier = AttributeModifier.ASSEMBLY;
            instance.Attributes.Add(attribute);
            return ref instance;
        }
    }
}