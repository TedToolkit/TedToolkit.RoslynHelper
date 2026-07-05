// -----------------------------------------------------------------------
// <copyright file="ConditionalCompilationExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The extensions for the <see cref="IConditionalCompilation"/>.
/// </summary>
public static class ConditionalCompilationExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IConditionalCompilation
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add conditional compilation expression.
        /// </summary>
        /// <param name="condition">Conditional expression.</param>
        /// <returns>self.</returns>
        /// <exception cref="ArgumentNullException">condition is null.</exception>
        public TItem AddCondition(PreprocessorExpression condition)
        {
            instance.Condition = condition ?? throw new ArgumentNullException(nameof(condition));
            return instance;
        }

        /// <summary>
        /// Add conditional compilation symbol.
        /// </summary>
        /// <param name="symbol">symbol name.</param>
        /// <returns>self.</returns>
        public TItem AddCondition(string symbol)
        {
            instance.Condition = PreprocessorExpression.Symbol(symbol);
            return instance;
        }

        internal void AddConditionalCompilationStart(ref SourceBuilder builder)
        {
            if (instance.Condition is null)
            {
                return;
            }

            builder.Append("#if ");
            instance.Condition.ToCode(ref builder);
            builder.AppendLine();
        }

        internal void AddConditionalCompilationEnd(ref SourceBuilder builder)
        {
            if (instance.Condition is null)
            {
                return;
            }

            builder.AppendLine();
            builder.Append("#endif");
        }
    }
}