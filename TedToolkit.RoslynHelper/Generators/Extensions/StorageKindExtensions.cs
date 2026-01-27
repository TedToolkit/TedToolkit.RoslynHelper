// -----------------------------------------------------------------------
// <copyright file="StorageKindExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IStorageKind"/>.
/// </summary>
public static class StorageKindExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IStorageKind
#pragma warning restore CA1034
    {
        /// <summary>
        /// Gets <see cref="StorageKind.IN"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem In
        {
            get
            {
                instance.StorageKind = StorageKind.IN;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="StorageKind.OUT"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem Out
        {
            get
            {
                instance.StorageKind = StorageKind.OUT;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="StorageKind.REF"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem Ref
        {
            get
            {
                instance.StorageKind = StorageKind.REF;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="StorageKind.REF_READONLY"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem RefReadonly
        {
            get
            {
                instance.StorageKind = StorageKind.REF_READONLY;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="StorageKind.SCOPED_IN"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem ScopedIn
        {
            get
            {
                instance.StorageKind = StorageKind.SCOPED_IN;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="StorageKind.SCOPED_REF"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem ScopedRef
        {
            get
            {
                instance.StorageKind = StorageKind.SCOPED_REF;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="StorageKind.SCOPED_REF_READONLY"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem ScopedRefReadonly
        {
            get
            {
                instance.StorageKind = StorageKind.SCOPED_REF_READONLY;
                return instance;
            }
        }

        internal void AddStorageKind(ref SourceBuilder builder)
        {
            if (instance.StorageKind is StorageKind.NONE)
                return;

            builder.Append(instance.StorageKind switch
            {
                StorageKind.IN => "in ",
                StorageKind.OUT => "out ",
                StorageKind.REF => "ref ",
                StorageKind.REF_READONLY => "ref readonly ",
                StorageKind.SCOPED_IN => "scoped in ",
                StorageKind.SCOPED_REF => "scoped ref ",
                StorageKind.SCOPED_REF_READONLY => "scoped ref readonly ",
                _ => throw new InvalidOperationException(nameof(instance.StorageKind)),
            });
        }
    }
}