// -----------------------------------------------------------------------
// <copyright file="AccessibilityExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IAccessibility"/>.
/// </summary>
public static class AccessibilityExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IAccessibility
#pragma warning restore CA1034
    {
        /// <summary>
        /// Gets <see cref="Accessibility.PUBLIC"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem Public
        {
            get
            {
                instance.Accessibility = Accessibility.PUBLIC;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="Accessibility.INTERNAL"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem Internal
        {
            get
            {
                instance.Accessibility = Accessibility.INTERNAL;
                return instance;
            }
        }

        /// <summary>
        /// <see cref="Accessibility.PRIVATE"/>Gets .
        /// </summary>
        /// <returns>item.</returns>
        public TItem Private
        {
            get
            {
                instance.Accessibility = Accessibility.PRIVATE;
                return instance;
            }
        }

        /// <summary>
        /// <see cref="Accessibility.FILE"/>Gets .
        /// </summary>
        /// <returns>item.</returns>
        public TItem File
        {
            get
            {
                instance.Accessibility = Accessibility.FILE;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="Accessibility.PRIVATE_PROTECTED"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem PrivateProtected
        {
            get
            {
                instance.Accessibility = Accessibility.PRIVATE_PROTECTED;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="Accessibility.PROTECTED"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem Protected
        {
            get
            {
                instance.Accessibility = Accessibility.PROTECTED;
                return instance;
            }
        }

        /// <summary>
        /// Gets <see cref="Accessibility.PROTECTED_INTERNAL"/>.
        /// </summary>
        /// <returns>item.</returns>
        public TItem ProtectedInternal
        {
            get
            {
                instance.Accessibility = Accessibility.PROTECTED_INTERNAL;
                return instance;
            }
        }

        internal void AddAccessibility(ref SourceBuilder builder)
        {
            if (instance.Accessibility is Accessibility.NONE)
            {
                return;
            }

            builder.Append(instance.Accessibility switch
            {
                Accessibility.PUBLIC => "public ",
                Accessibility.INTERNAL => "internal ",
                Accessibility.PRIVATE => "private ",
                Accessibility.FILE => "file ",
                Accessibility.PRIVATE_PROTECTED => "private protected ",
                Accessibility.PROTECTED => "protected ",
                Accessibility.PROTECTED_INTERNAL => "protected internal ",
                _ => throw new InvalidOperationException(nameof(instance.Accessibility)),
            });
        }
    }
}