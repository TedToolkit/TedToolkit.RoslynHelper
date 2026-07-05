// -----------------------------------------------------------------------
// <copyright file="PolymorphismExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The extensions for the <see cref="Polymorphism"/>.
/// </summary>
public static class PolymorphismExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IPolymorphism
#pragma warning restore CA1034
    {
        /// <summary>
        /// <see cref="Polymorphism.ABSTRACT"/>Gets .
        /// </summary>
        /// <returns>item.</returns>
        public TItem Abstract
        {
            get
            {
                instance.Polymorphism = Polymorphism.ABSTRACT;
                return instance;
            }
        }

        /// <summary>
        /// <see cref="Polymorphism.VIRTUAL"/>Gets .
        /// </summary>
        /// <returns>item.</returns>
        public TItem Virtual
        {
            get
            {
                instance.Polymorphism = Polymorphism.VIRTUAL;
                return instance;
            }
        }

        /// <summary>
        /// <see cref="Polymorphism.OVERRIDE"/>Gets .
        /// </summary>
        /// <returns>item.</returns>
        public TItem Override
        {
            get
            {
                instance.Polymorphism = Polymorphism.OVERRIDE;
                return instance;
            }
        }

        /// <summary>
        /// <see cref="Polymorphism.SEALED"/>Gets .
        /// </summary>
        /// <returns>item.</returns>
        public TItem Sealed
        {
            get
            {
                instance.Polymorphism = Polymorphism.SEALED;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="Polymorphism.NEW"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem New
        {
            get
            {
                instance.Polymorphism = Polymorphism.NEW;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="Polymorphism.SEALED_OVERRIDE"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem SealedOverride
        {
            get
            {
                instance.Polymorphism = Polymorphism.SEALED_OVERRIDE;
                return instance;
            }
        }

        internal void AddPolymorphism(ref SourceBuilder builder)
        {
            if (instance.Polymorphism is Polymorphism.NONE)
            {
                return;
            }

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