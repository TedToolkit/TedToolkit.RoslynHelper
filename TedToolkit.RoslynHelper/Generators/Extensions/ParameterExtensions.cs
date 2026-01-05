// -----------------------------------------------------------------------
// <copyright file="ParameterExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The parameter extensions
/// </summary>
public static class ParameterExtensions
{
#pragma warning disable CA1034
    extension(ref Parameter instance)
#pragma warning restore CA1034
#pragma warning disable S2325
    {
        /// <summary>
        /// Add default
        /// </summary>
        public ref Parameter AddDefault()
        {
            instance.Default = new SimpleNameExpression("default");
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(IExpression value)
        {
            instance.Default = value;
            return ref instance;
        }
    }
#pragma warning restore S2325
}