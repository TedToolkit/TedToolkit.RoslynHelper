// -----------------------------------------------------------------------
// <copyright file="TypeDeclarationExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for <see cref="TypeDeclaration"/>
/// </summary>
public static class TypeDeclarationExtensions
{
#pragma warning disable CA1034
    extension(ref TypeDeclaration instance)
#pragma warning restore CA1034
#pragma warning disable S2325
    {
        /// <summary>
        /// Add the baseType.
        /// </summary>
        /// <param name="baseType">the baseType</param>
        /// <returns>the item</returns>
        public ref TypeDeclaration AddBaseType(MemberAccess baseType)
        {
            instance.BaseTypes.Add(baseType);
            return ref instance;
        }

        /// <summary>
        /// Add the baseType.
        /// </summary>
        /// <typeparam name="T">BaseType</typeparam>
        /// <returns>the item</returns>
        public ref TypeDeclaration AddBaseType<T>()
        {
            instance.BaseTypes.Add(SourceComposer.Type<T>());
            return ref instance;
        }

        /// <summary>
        /// Add the baseType.
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>the item</returns>
        public ref TypeDeclaration AddBaseType(Type type)
        {
            instance.BaseTypes.Add(SourceComposer.Type(type));
            return ref instance;
        }
    }
#pragma warning restore S2325
}