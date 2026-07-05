// -----------------------------------------------------------------------
// <copyright file="IConditionalCompilation.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// Represents a syntax item that can be wrapped in conditional compilation directives.
/// </summary>
public interface IConditionalCompilation
{
    /// <summary>
    /// Gets or sets the preprocessor condition used by the surrounding <c>#if</c> block.
    /// </summary>
    PreprocessorExpression? Condition { get; set; }
}