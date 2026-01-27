// -----------------------------------------------------------------------
// <copyright file="DefaultExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IDefault"/>.
/// </summary>
public static class DefaultExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IDefault
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add default.
        /// </summary>
        /// <param name="value">defaultValue.</param>
        /// <returns>self.</returns>
        public TItem AddDefault(IExpression value)
        {
            instance.Default = value;
            return instance;
        }

        /// <summary>
        /// Add null.
        /// </summary>
        /// <returns>self.</returns>
        public TItem AddNull()
        {
            instance.Default = SimpleNameExpression.Null;
            return instance;
        }

        /// <summary>
        /// Add default.
        /// </summary>
        /// <returns>self.</returns>
        public TItem AddDefault()
        {
            instance.Default = SimpleNameExpression.Default;
            return instance;
        }

        internal void AddDefault(ref SourceBuilder builder)
        {
            if (instance.Default is null)
                return;

            builder.Append(" = ");
            instance.Default.ToCode(ref builder);
        }
    }
}