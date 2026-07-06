// -----------------------------------------------------------------------
// <copyright file="PreprocessorSymbolExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

/// <summary>
/// Represents a single preprocessor symbol.
/// </summary>
/// <param name="text">The symbol text.</param>
internal sealed class PreprocessorSymbolExpression(string text) : PreprocessorExpression
{
    /// <inheritdoc />
    internal override void WriteTo(ref SourceBuilder builder)
    {
        builder.Append(text);
    }
}