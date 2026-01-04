// -----------------------------------------------------------------------
// <copyright file="PolymorphismExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="Polymorphism"/>
/// </summary>
public static class PolymorphismExtensions
{
#pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IPolymorphism
#pragma warning restore CA1034
    {
        /// <summary>
        /// <see cref="Polymorphism.ABSTRACT"/>
        /// </summary>
        /// <returns>item</returns>
        public ref TItem Abstract
        {
            get
            {
                instance.Polymorphism = Polymorphism.ABSTRACT;
                return ref instance;
            }
        }

        /// <summary>
        /// <see cref="Polymorphism.VIRTUAL"/>
        /// </summary>
        /// <returns>item</returns>
        public ref TItem Virtual
        {
            get
            {
                instance.Polymorphism = Polymorphism.VIRTUAL;
                return ref instance;
            }
        }

        /// <summary>
        /// <see cref="Polymorphism.OVERRIDE"/>
        /// </summary>
        /// <returns>item</returns>
        public ref TItem Override
        {
            get
            {
                instance.Polymorphism = Polymorphism.OVERRIDE;
                return ref instance;
            }
        }

        /// <summary>
        /// <see cref="Polymorphism.SEALED"/>
        /// </summary>
        /// <returns>item</returns>
        public ref TItem Sealed
        {
            get
            {
                instance.Polymorphism = Polymorphism.SEALED;
                return ref instance;
            }
        }

        /// <summary>
        /// <see cref="Polymorphism.NEW"/>
        /// </summary>
        /// <returns>item</returns>
        public ref TItem New
        {
            get
            {
                instance.Polymorphism = Polymorphism.NEW;
                return ref instance;
            }
        }

        /// <summary>
        /// <see cref="Polymorphism.SEALED_OVERRIDE"/>
        /// </summary>
        /// <returns>item</returns>
        public ref TItem SealedOverride
        {
            get
            {
                instance.Polymorphism = Polymorphism.SEALED_OVERRIDE;
                return ref instance;
            }
        }

        internal void AddPolymorphism(ref SourceBuilder builder)
        {
            if (instance.Polymorphism is Polymorphism.NONE)
                return;

            builder.Append(instance.Polymorphism switch
            {
                Polymorphism.VIRTUAL => "virtual ",
                Polymorphism.ABSTRACT => "abstract ",
                Polymorphism.OVERRIDE => "override ",
                Polymorphism.SEALED => "sealed ",
                Polymorphism.NEW => "new ",
                Polymorphism.SEALED_OVERRIDE => "sealed override ",
                _ => throw new InvalidOperationException(nameof(instance.Polymorphism)),
            });
        }
    }
}