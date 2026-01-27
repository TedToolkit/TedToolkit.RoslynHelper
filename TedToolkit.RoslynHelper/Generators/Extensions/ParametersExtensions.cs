// -----------------------------------------------------------------------
// <copyright file="ParametersExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IParameters"/>.
/// </summary>
public static class ParametersExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IParameters
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add parameter.
        /// </summary>
        /// <param name="parameter">the parameter.</param>
        public TItem AddParameter(Parameter parameter)
        {
            instance.Parameters.Add(parameter);
            return instance;
        }

        internal void AddParameters(ref SourceBuilder builder)
        {
            if (instance.Parameters.Count is 0)
                return;

            instance.AddParametersNoSkip(ref builder);
        }

        internal void AddParametersNoSkip(ref SourceBuilder builder)
        {
            builder.Append('(');
            instance.AddParametersList(ref builder);
            builder.Append(')');
        }

        internal void AddParametersList(ref SourceBuilder builder)
        {
            var isNotStart = false;
            foreach (var parameter in instance.Parameters)
            {
                if (isNotStart)
                    builder.AppendLine(',');
                else
                    builder.AppendLine();

                builder.Append('\t');
                parameter.ToCode(ref builder);

                isNotStart = true;
            }
        }
    }
}