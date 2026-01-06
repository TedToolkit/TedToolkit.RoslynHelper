// -----------------------------------------------------------------------
// <copyright file="TypeParametersExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="ITypeParameters"/>
/// </summary>
public static class TypeParametersExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, ITypeParameters
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add type parameter
        /// </summary>
        /// <param name="typeParameter">the type parameter</param>
        public TItem AddTypeParameter(TypeParameter typeParameter)
        {
            instance.TypeParameters.Add(typeParameter);
            return instance;
        }

        internal void AddTypeParameterConstraints(ref SourceBuilder builder)
        {
            foreach (var instanceTypeParameter in instance.TypeParameters)
                instanceTypeParameter.ToConstraint(ref builder);
        }

        internal void AddTypeParameters(ref SourceBuilder builder)
        {
            if (instance.TypeParameters.Count is 0)
                return;

            instance.AddTypeParametersNoSkip(ref builder);
        }

        internal void AddTypeParametersNoSkip(ref SourceBuilder builder)
        {
            builder.Append('<');
            var isNotStart = false;
            foreach (var parameter in instance.TypeParameters)
            {
                if (isNotStart)
                    builder.AppendLine(',');
                else
                    builder.AppendLine();

                builder.Append('\t');
                parameter.ToCode(ref builder);

                isNotStart = true;
            }

            builder.Append('>');
        }
    }
}