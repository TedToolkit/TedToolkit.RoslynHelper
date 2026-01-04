// -----------------------------------------------------------------------
// <copyright file="ArgumentExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Types;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IArguments"/>
/// </summary>
public static class ArgumentExtensions
{
#pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IArguments
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add argument
        /// </summary>
        /// <param name="argument">argument</param>
        /// <returns>the item</returns>
        public ref TItem AddArgument(Argument argument)
        {
            instance.Arguments.Add(argument);
            return ref instance;
        }

        internal void AddArguments(ref SourceBuilder builder)
        {
            if (instance.Arguments.Count == 0)
                return;

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
                    builder.Append(", ");

                attribute.ToCode(ref builder);
                isNotStart = true;
            }
        }
    }
}