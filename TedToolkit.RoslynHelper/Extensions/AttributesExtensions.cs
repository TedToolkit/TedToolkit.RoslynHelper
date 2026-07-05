// -----------------------------------------------------------------------
// <copyright file="AttributesExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom.Compiler;

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The extensions for the <see cref="IAttributes"/>.
/// </summary>
public static class AttributesExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IAttributes
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add attribute.
        /// </summary>
        /// <param name="attribute">attribute.</param>
        /// <param name="condition">the optional conditional compilation expression.</param>
        /// <returns>the item.</returns>
        public TItem AddAttribute(Syntaxes.Attribute attribute, Syntaxes.Preprocessors.PreprocessorExpression? condition = null)
        {
            instance.Attributes.Add(new Syntaxes.ConditionalItem<Syntaxes.Attribute>(attribute, condition));
            return instance;
        }

        /// <summary>
        /// Add generator attribute.
        /// </summary>
        /// <param name="type">type.</param>
        /// <returns>self.</returns>
        /// <exception cref="ArgumentNullException">type is null.</exception>
        public TItem AddGeneratorAttribute(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return instance.AddGeneratorAttribute(type.GetToolName(), type.GetVersion());
        }

        /// <summary>
        /// Add generator attribute.
        /// </summary>
        /// <param name="toolName">tool name.</param>
        /// <param name="version">version.</param>
        /// <returns>self.</returns>
        /// <exception cref="ArgumentNullException">toolName or version is null.</exception>
        public TItem AddGeneratorAttribute(string toolName, string version)
        {
            return instance.AddGeneratorAttribute(
                toolName?.ToLiteral() ?? throw new ArgumentNullException(nameof(toolName)),
                version?.ToLiteral() ?? throw new ArgumentNullException(nameof(version)));
        }

        /// <summary>
        /// Add generator attribute.
        /// </summary>
        /// <param name="toolName">tool name.</param>
        /// <param name="version">version.</param>
        /// <returns>self.</returns>
        public TItem AddGeneratorAttribute(IExpression toolName, IExpression version)
        {
            instance.AddAttribute(new Syntaxes.Attribute(typeof(GeneratedCodeAttribute))
                .AddArgument(new Argument(toolName))
                .AddArgument(new Argument(version)));
            return instance;
        }

        internal void AddAttributes(ref SourceBuilder builder)
        {
            if (instance.Attributes.Count == 0)
            {
                return;
            }

            foreach (var conditionalAttribute in instance.Attributes.AsSpan())
            {
                if (conditionalAttribute.Condition is not null)
                {
                    builder.Append("#if ");
                    conditionalAttribute.Condition.ToCode(ref builder);
                    builder.AppendLine();
                }

                builder.Append('[');
                conditionalAttribute.Item.ToCode(ref builder);
                builder.Append(']');
                builder.AppendLine();

                if (conditionalAttribute.Condition is not null)
                {
                    builder.Append("#endif");
                    builder.AppendLine();
                }
            }
        }
    }
}