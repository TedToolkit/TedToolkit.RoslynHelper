// -----------------------------------------------------------------------
// <copyright file="ArgumentExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The extensions for the <see cref="IArguments"/>.
/// </summary>
public static class ArgumentExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IArguments
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add argument.
        /// </summary>
        /// <param name="argument">argument.</param>
        /// <returns>the item.</returns>
        public TItem AddArgument(Argument argument)
        {
            instance.Arguments.Add(argument);
            return instance;
        }

        /// <summary>
        /// Adds a positional argument expression.
        /// </summary>
        /// <param name="argument">The argument expression.</param>
        /// <returns>The item.</returns>
        public TItem AddArgument(IExpression argument)
        {
            return instance.AddArgument(new Argument(argument));
        }

        /// <summary>
        /// Adds positional argument expressions in the supplied order.
        /// </summary>
        /// <param name="arguments">The argument expressions.</param>
        /// <returns>The item.</returns>
        /// <exception cref="ArgumentNullException">The argument array is null.</exception>
        public TItem AddArguments(params IExpression[] arguments)
        {
            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            foreach (var argument in arguments)
            {
                instance.AddArgument(argument);
            }

            return instance;
        }

        internal void AddArguments(ref SourceBuilder builder)
        {
            if (instance.Arguments.Count == 0)
            {
                return;
            }

            instance.AddArgumentsNoSkip(ref builder);
        }

        internal void AddArgumentsNoSkip(ref SourceBuilder builder)
        {
            builder.Append('(');
            instance.AddArgumentList(ref builder);
            builder.Append(')');
        }

        internal void AddArgumentList(ref SourceBuilder builder)
        {
            var isNotStart = false;
            foreach (var attribute in instance.Arguments.AsSpan())
            {
                if (isNotStart)
                {
                    builder.Append(", ");
                }

                attribute.ToCode(ref builder);
                isNotStart = true;
            }
        }
    }
}