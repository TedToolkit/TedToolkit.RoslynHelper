// -----------------------------------------------------------------------
// <copyright file="ParametersExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Types;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IParameters"/>
/// </summary>
public static class ParametersExtensions
{
#pragma warning disable CA1034
    extension<TItem>(ref TItem instance)
        where TItem : struct, IParameters
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add parameters
        /// </summary>
        /// <param name="parameter">the parameter</param>
        public ref TItem AddParameter(Parameter parameter)
        {
            instance.Parameters.Add(parameter);
            return ref instance;
        }

        internal void AddParameters(ref SourceBuilder builder)
        {
            if (instance.Parameters.Count is 0)
                return;

            builder.Append('(');
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

            builder.Append(')');
        }

        internal void AddParametersSummary(ref SourceBuilder builder)
        {
            if (instance.Parameters.Count is 0)
                return;

#pragma warning disable RCS1264
            foreach (ref var instanceParameter in instance.Parameters.AsSpan())
#pragma warning restore RCS1264
            {
                if (instanceParameter.Description.Count is 0)
                    continue;

                builder.Append("/// <param name=\"@");
                builder.Append(instanceParameter.Identifier);
                builder.AppendLine("\">");
                instanceParameter.AddDescriptionItems(ref builder);
                builder.AppendLine("/// </param>");
            }
        }
    }
}